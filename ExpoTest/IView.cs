using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ExpoTest
{
    public interface IView
    {
        void AddTextBoxToGrid(Control control);
    }
}
