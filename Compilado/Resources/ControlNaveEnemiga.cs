using OpenTK.Math;
using OpenTK.Graphics;
using Fang3D;

public class ControlNaveEnemiga : Script
{
	public float velocidadX = 10;
	public float velocidadY = 10;
	public int disparosPorSegundo = 1;
	
	public override void Update()
	{
		/*Vector3 delta = Vector3.Zero;
	
		if (delta != Vector3.Zero)
			Transformation.LocalTranslation += delta * Time.DeltaTime;
			
		if (retrasoDisparo > 0)
			retrasoDisparo -= Time.DeltaTime;
			
		if (Input.IsKeyDown(KeyEnum.Space))
			disparar();*/
	}
	
	public override void Collision(Collisionable col)
	{
		if (!Destroyed)
		{
			Entity entPs = ComponentFactory.NewParticleSystem();
			entPs.Transformation.LocalTranslation = Transformation.GlobalTranslation;
			
			ParticleSystem ps =  (ParticleSystem) entPs.FindChildComponent(typeof(ParticleSystem));
			
			ps.MaxSize = 0.1f;
			ps.MinSize = 0.01f;
			ps.AutoDestroy = true;
			ps.Burst = true;
			ps.MaxParticleCount = 200;
		
			Entity.Destroy();
		}
	}
}
