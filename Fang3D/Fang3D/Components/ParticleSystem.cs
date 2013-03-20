using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Math;
using OpenTK.Graphics;
using Fang3D.Attributes;

namespace Fang3D
{
    public class ParticleSystem : ScriptBase
    {
        private int maxParticleCount = 30;
        private float minLife = 0.5f;
        private float maxLife = 1.0f;
        private float minSpeed = 1.0f;
        private float maxSpeed = 5.0f;
        private float minSize = 0.1f;
        private float maxSize = 0.2f;
        private bool autoDestroy = false;
        private bool useLighting = false;
        private bool burst = false;
        private Color4 color = new Color4(1.0f, 1.0f, 1.0f, 1.0f);
        private bool globalPosition = false;
        private int particlesPerSecond = 10;
        private Vector3 direction;
        private float spread;

        public int ParticlesPerSecond
        {
            get { return particlesPerSecond; }
            set { particlesPerSecond = value; }
        }

        public bool GlobalPosition
        {
            get { return globalPosition; }
            set { globalPosition = value; }
        }

        public bool Burst
        {
            get { return burst; }
            set { burst = value; }
        }

        public Color4 Color
        {
            get { return color; }
            set { color = value; }
        }

        public int MaxParticleCount
        {
            get { return maxParticleCount; }
            set { maxParticleCount = value; }
        }

        public float MinLife
        {
            get { return minLife; }
            set { minLife = value; }
        }

        public float MaxLife
        {
            get { return maxLife; }
            set { maxLife = value; }
        }
        
        public float MinSpeed
        {
            get { return minSpeed; }
            set { minSpeed = value; }
        }
        
        public float MaxSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = value; }
        }
        
        public float MinSize
        {
            get { return minSize; }
            set { minSize = value; }
        }
        
        public float MaxSize
        {
            get { return maxSize; }
            set { maxSize = value; }
        }
        
        public bool AutoDestroy
        {
            get { return autoDestroy; }
            set { autoDestroy = value; }
        }

        public bool UseLighting
        {
            get { return useLighting; }
            set { useLighting = value; }
        }

        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public float Spread
        {
            get { return spread; }
            set { spread = value; }
        }


        [DontStore]
        [DontShowInEditor]
        internal Particle[] particles;
        private int activeParticles;
        private float timeCreationParticles;

        private System.Random rnd = new Random();

        public override void Start()
        {
            particles = new Particle[MaxParticleCount];

            if (Burst)
            {
                for (int i = 0; i < MaxParticleCount; i++)
                    particles[i] = CreateParticle();
            }
        }

        private Particle CreateParticle()
        {
            Particle part = new Particle();

            if (Burst == true || particlesPerSecond > 0 && timeCreationParticles > 1.0f / (float)particlesPerSecond)
            {
                part.color = ((uint)(color.A * 255) & 0xFF) << 24 |
                             ((uint)(color.B * 255) & 0xFF) << 16 |
                             ((uint)(color.G * 255) & 0xFF) << 8 |
                             ((uint)(color.R * 255) & 0xFF) << 0;

                if (globalPosition)
                    part.position = Entity.Transformation.GlobalTranslation;
                part.life = (float)rnd.NextDouble() * (MaxLife - MinLife) + MinLife;
                part.size = (float)rnd.NextDouble() * (MaxSize - MinSize) + MinSize;

                Vector3 partDirection;

                if (direction == Vector3.Zero)
                {
                    partDirection = Vector3.Normalize(new Vector3((float)rnd.NextDouble() - 0.5f, (float)rnd.NextDouble() - 0.5f, (float)rnd.NextDouble() - 0.5f));
                }
                else
                {
                    float spreadX = Functions.DegreesToRadians(((float)rnd.NextDouble() - 0.5f) * spread);
                    float spreadY = Functions.DegreesToRadians(((float)rnd.NextDouble() - 0.5f) * spread);
                    float spreadZ = Functions.DegreesToRadians(((float)rnd.NextDouble() - 0.5f) * spread);

                    partDirection = Vector3.Normalize(direction);

                    partDirection = Vector3.Transform(partDirection, Matrix4.RotateX(spreadX)).Xyz;
                    partDirection = Vector3.Transform(partDirection, Matrix4.RotateY(spreadY)).Xyz;
                    partDirection = Vector3.Transform(partDirection, Matrix4.RotateZ(spreadZ)).Xyz;
                }

                part.speed = partDirection * ((float)rnd.NextDouble() * (MaxSpeed - MinSpeed) + MinSpeed);

                activeParticles++;

                timeCreationParticles -= 1.0f / (float)particlesPerSecond;
            }

            return part;
        }

        public override void Update()
        {
            timeCreationParticles += Time.DeltaTime;

            if (activeParticles > 0 || Burst == false)
            {
                if (particles.Length != MaxParticleCount)
                    Array.Resize<Particle>(ref particles, MaxParticleCount);

                for (int i = 0; i < particles.Length; i++)
                {
                    if (particles[i].life > 0)
                    {
                        particles[i].life -= Time.DeltaTime;
                        if (particles[i].life <= 0)
                        {
                            activeParticles--;

                            if (Burst == false)
                                particles[i] = CreateParticle();
                            else
                                particles[i].life = 0;
                        }
                        else
                        {
                            particles[i].position += particles[i].speed * Time.DeltaTime;
                        }
                    }
                    else
                    {
                        if (Burst == false)
                            particles[i] = CreateParticle();
                    }
                }
            }
            else
            {
                if (AutoDestroy)
                    Entity.Destroy();
            }
        }
    }

    public struct Particle
    {
        public Vector3 position;
        public uint color;
        public float size;
        public Vector3 speed;
        public float life;
    }
}
