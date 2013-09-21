using System;

using System.Collections.Generic;
using System.Text;

namespace Fluid.Classes
{
    internal class ListBoxAnimation:Animation
    {
        public int Index { get; set; }
        public object Item { get; set; }

        public static void AnimateModal(int index, int beginValue, int endValue, int duration, AnimationFunc func)
        {
            using (ListBoxAnimation a = new ListBoxAnimation())
            {
                a.Index = index;
                a.BeginValue = beginValue;
                a.EndValue = endValue;
                a.Duration = duration;
                a.Scene +=new EventHandler<AnimationEventArgs>(func);
                a.Mode = AnimationMode.Log;
                a.Acceleration = 0.05f;
                a.StartModal();
            }
        }
    }
}
