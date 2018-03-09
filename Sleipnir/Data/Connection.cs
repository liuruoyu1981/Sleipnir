using System;

namespace Sleipnir
{
    [Serializable]
    public class Connection {
        public INode A;
        public INode B;
        
        public Connection(INode a, INode b)
        {
            A = a;
            B = b;
        }

        public override bool Equals(System.Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType()) 
                return false;

            Connection other = (Connection)obj;
            return (A == other.A && B == other.B);
        }
        
        public override int GetHashCode()
        {
            return A.GetHashCode() + B.GetHashCode();
        }
        
        public override string ToString()
        {
            return string.Format("{0} => {1}", A, B);
        }
    }
}