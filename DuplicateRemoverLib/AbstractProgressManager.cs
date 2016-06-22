using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateRemoverLib
{
    public abstract class AbstractProgressManager
    {

        public double Value { get; protected set; }

        public string Status { get; protected set; }

        public bool Active { get; private set; }

        abstract public void Start();

        abstract public void Stop();

        public void Update(double value, string status = "")
        {
            Value = value;
            Status = status;
        }

    }
}
