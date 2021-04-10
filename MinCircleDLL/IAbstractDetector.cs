using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MinCircleDLL
{
    interface IAbstractDetector
    {
        UserControl GetUserControl(string csvFileName);
        string Feature { get; set; }
        int CurrentLineIndex { set; }
    }
}
