using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourEngine
{
    public struct Move
    {
        public static readonly Move NullMove = new();
        public int moveValue;
        public Move(int moveValue)
        {
            this.moveValue = moveValue;
        }
        
    }
    
}
