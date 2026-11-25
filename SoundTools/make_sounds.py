# Penguin Ice Run — all sound effects and both music tracks, generated from code (own work, CC0).
# Run: python3 make_sounds.py <output folder>
import sys, os, wave
import numpy as np

SR = 44100
OUT = sys.argv[1] if len(sys.argv) > 1 else "."
rng = np.random.default_rng(7)


def t_of(sec):
    return np.arange(int(SR * sec)) / SR


def env(n, attack=0.005, release=0.1, sec=None):
    a = min(n // 2, max(1, int(SR * attack))); r = min(n // 2, max(1, int(SR * release)))
    e = np.ones(n)
    e[:a] = np.linspace(0, 1, a)
    e[-r:] *= np.linspace(1, 0, r)
    return e


def tone(freq_start, freq_end, sec, shape="square", vol=0.5, attack=0.005, release=0.08):
    t = t_of(sec)
    f = np.linspace(freq_start, freq_end, len(t))
    phase = 2 * np.pi * np.cumsum(f) / SR
    if shape == "square":
        w = np.sign(np.sin(phase)) * 0.6
    elif shape == "tri":
        w = 2 / np.pi * np.arcsin(np.sin(phase))
    elif shape == "saw":
        w = 2 * ((phase / (2 * np.pi)) % 1) - 1
    else:
        w = np.sin(phase)
    return w * env(len(t), attack, release) * vol


def noise(sec, vol=0.5, decay=8.0):
    t = t_of(sec)
    return rng.uniform(-1, 1, len(t)) * np.exp(-t * decay) * vol


def lowpass(x, alpha):
    y = np.zeros_like(x); acc = 0.0
    for i, v in enumerate(x):
        acc += alpha * (v - acc); y[i] = acc
    return y


def mix(*parts):
    n = max(len(p) for p in parts)
    out = np.zeros(n)
    for p in parts:
        out[:len(p)] += p
    return out


def cat(*parts):
    return np.concatenate(parts)


def save(name, x, peak=0.85):
    x = np.asarray(x, dtype=np.float64)
    m = np.max(np.abs(x)) or 1.0
    x = (x / m * peak * 32767).astype(np.int16)
    with wave.open(os.path.join(OUT, name + ".wav"), "wb") as w:
        w.setnchannels(1); w.setsampwidth(2); w.setframerate(SR); w.writeframes(x.tobytes())


# ---------- sound effects ----------
save("Jump", tone(330, 880, 0.16, "square", release=0.06))
save("Throw", mix(lowpass(noise(0.18, 1.0, 18), 0.25), tone(600, 300, 0.12, "sine", 0.3)))
save("Pickup", cat(tone(988, 988, 0.07, "square"), tone(1319, 1319, 0.16, "square", release=0.12)))
save("Hurt", mix(tone(400, 110, 0.28, "saw", release=0.1), lowpass(noise(0.2, 0.6, 14), 0.3)))
save("BossShoot", mix(tone(900, 200, 0.22, "square", release=0.1), lowpass(noise(0.22, 0.5, 10), 0.15)))
save("WindUp", tone(220, 660, 0.42, "saw", 0.5, attack=0.03, release=0.05)
     * (0.6 + 0.4 * np.sin(2 * np.pi * 18 * t_of(0.42))))                        # rising "warning" buzz
save("EnemyDeath", mix(cat(tone(660, 330, 0.08, "square"), tone(330, 80, 0.22, "square", release=0.15)),
                       lowpass(noise(0.3, 0.6, 12), 0.2)))
save("Click", tone(1500, 1200, 0.04, "sine", 0.7, attack=0.001, release=0.03))
save("Win", cat(*[tone(f, f, d, "square", release=0.05) for f, d in
                 [(523, 0.12), (659, 0.12), (784, 0.12), (1047, 0.35)]]))
save("Lose", cat(*[tone(f, f * 0.97, d, "tri", release=0.06) for f, d in
                  [(392, 0.18), (330, 0.18), (262, 0.18), (196, 0.45)]]))
save("PortalOpen", mix(tone(300, 1200, 0.5, "sine", 0.5, attack=0.02, release=0.2),
                       tone(450, 1800, 0.5, "tri", 0.25, attack=0.02, release=0.2)))


# ---------- music (loops) ----------
NOTE = {n: 440 * 2 ** ((i - 9) / 12) for i, n in enumerate(
    ["C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"])}


def f(name, octave):
    return NOTE[name] * 2 ** (octave - 4)


def track(melody, bass, bpm, shape_lead, shape_bass, drums=True):
    beat = 60 / bpm
    total = int(SR * beat * sum(d for _, d in melody))
    out = np.zeros(total)
    pos = 0
    for n, d in melody:
        seg = int(SR * beat * d)
        if n:
            out[pos:pos + seg] += tone(n, n, beat * d, shape_lead, 0.32, 0.01, 0.06)[:seg]
        pos += seg
    pos = 0
    for n, d in bass:
        seg = int(SR * beat * d)
        if n and pos < total:
            s = tone(n, n, beat * d, shape_bass, 0.38, 0.01, 0.05)[:seg]
            out[pos:pos + len(s)] += s[: total - pos]
        pos += seg
    if drums:
        k = int(SR * beat)
        for i in range(0, total, k):
            kick = tone(120, 45, 0.12, "sine", 0.6, 0.001, 0.08)
            out[i:i + len(kick)] += kick[: total - i]
            h = i + k // 2
            if h < total:
                hat = lowpass(noise(0.05, 0.15, 60), 0.6)
                out[h:h + len(hat)] += hat[: total - h]
    return out


# level: upbeat C major, 132 bpm, 16 bars
lead_lv = []
for bar in range(4):
    lead_lv += [(f("C", 5), .5), (f("E", 5), .5), (f("G", 5), .5), (f("E", 5), .5),
                (f("A", 5), .5), (f("G", 5), .5), (f("E", 5), 1),
                (f("F", 5), .5), (f("A", 5), .5), (f("G", 5), .5), (f("E", 5), .5),
                (f("D", 5), .5), (f("E", 5), .5), (f("C", 5), 1)]
    lead_lv += [(f("E", 5), .5), (f("G", 5), .5), (f("C", 6), 1), (f("B", 5), .5), (f("G", 5), .5), (f("A", 5), 1),
                (f("F", 5), .5), (f("E", 5), .5), (f("D", 5), 1), (f("G", 4), .5), (f("B", 4), .5), (f("C", 5), 1)]
bass_lv = []
for bar in range(8):
    for root in ["C", "A", "F", "G"]:
        bass_lv += [(f(root, 2), .5), (f(root, 3), .5)] * 2
save("Music", track(lead_lv, bass_lv, 132, "square", "tri"), peak=0.7)

# menu: calm A minor, 92 bpm, no drums
lead_mn = []
for rep in range(4):
    lead_mn += [(f("A", 4), 1), (f("C", 5), 1), (f("E", 5), 1), (f("D", 5), 1),
                (f("C", 5), 1), (f("B", 4), 1), (f("A", 4), 2),
                (f("F", 4), 1), (f("A", 4), 1), (f("C", 5), 1), (f("B", 4), 1),
                (f("G", 4), 1), (f("B", 4), 1), (f("E", 5), 2)]
bass_mn = []
for rep in range(4):
    for root in ["A", "F", "D", "E"]:
        bass_mn += [(f(root, 2), 2), (f(root, 3), 2)]
save("MenuMusic", track(lead_mn, bass_mn, 92, "tri", "sine", drums=False), peak=0.7)
print("done")
