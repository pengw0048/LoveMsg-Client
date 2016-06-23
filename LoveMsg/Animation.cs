using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LoveMsg
{
    class Animation
    {
        private int set = 0, stage = 0, step = 0;
        private Random r = new Random();
        public double probswit;
        private List<List<Color>> sets = new List<List<Color>>();
        private List<int> steps = new List<int>();
        public Animation(double probswit=0.25)
        {
            this.probswit = probswit;
            sets.Add(new List<Color>() { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Purple });
            steps.Add(16);
            sets.Add(new List<Color>() { Color.Red, Color.White, Color.Orange, Color.White, Color.Yellow, Color.White, Color.Green, Color.White, Color.Blue, Color.White, Color.Purple, Color.White });
            steps.Add(16);
        }
        public Color Next()
        {
            int nset = set, nstage = stage;
            bool gonext = false;
            nstage++;
            if (nstage == sets[set].Count)
            {
                nstage = 0;
                if (step >= steps[set] - 1 && r.NextDouble() < probswit)
                {
                    nset++;
                    gonext = true;
                }
                if (nset == sets.Count) nset = 0;
            }
            Color a = sets[set][stage], b = sets[nset][nstage];
            double p = 1 - (double)step / steps[set];
            step++;
            if (step == steps[set])
            {
                step = 0;
                stage++;
                if (stage == sets[set].Count)
                {
                    stage = 0;
                    if (gonext) set++;
                    if (set == sets.Count) set = 0;
                }
            }
            return Color.FromArgb((int)(a.R * p + b.R * (1 - p)), (int)(a.G * p + b.G * (1 - p)), (int)(a.B * p + b.B * (1 - p)));
        }
    }
}
