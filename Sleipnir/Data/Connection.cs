using System;

namespace Sleipnir
{
    [Serializable]
    public class Connection {
        public string ID;
        public Node A;
        public Node B;
        
        public Connection(Node a, Node b)
        {
            ID = Guid.NewGuid().ToString();
            A = a;
            B = b;
        }
    }
}