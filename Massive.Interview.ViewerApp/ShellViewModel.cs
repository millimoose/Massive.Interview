using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Massive.Interview.ViewerApp
{
    public class ShellViewModel : PropertyChangedBase
    {
        string _hello = "Hello";

        public string Hello {
            get => _hello;
            set {
                _hello = value;
                NotifyOfPropertyChange(() => Hello);
            }
        }
    }
}
