using System;
using System.Collections.Generic;
using System.Text;

namespace Fang3D
{
    public class PhysicsEngine
    {
        public class CollisionPair
        {
            public Collisionable col1;
            public Collisionable col2;

            public CollisionPair(Collisionable col1, Collisionable col2)
            {
                this.col1 = col1;
                this.col2 = col2;
            }
        }

        internal CollisionPair[] Update(float deltaTime)
        {
            List<CollisionPair> newCollisions = new List<CollisionPair>();

            foreach (Collisionable col1 in Component.AllComponentsOfType(typeof(Collisionable)))
                if (!col1.Destroyed && col1.Enabled)
                    foreach (Collisionable col2 in Component.AllComponentsOfType(typeof(Collisionable)))
                        if (col2 != col1)
                            if (!col2.Destroyed && col2.Enabled && col2.CollidesWith(col1))
                                newCollisions.Add(new CollisionPair(col1, col2));

            return newCollisions.ToArray();
        }
    }
}
