using System;
using System.Collections.Generic;
using System.Text;
using SEProject2.Models;

namespace SEProject2.Domain
{
    public abstract class Command
    {
        public abstract void Execute(Character c);
    }
}
