using OpenTK.Math;
using OpenTK.Graphics;
using Fang3D;

public class ControlTeclasYDisparo : Script
{
	public float velocidadX = 10;
	public float velocidadY = 10;
	public int disparosPorSegundo = 1;
	
	private float retrasoDisparo = 0;
	private float rotacionAcumulada = 0;

	public override void Update()
	{
		Vector3 delta = Vector3.Zero;
	
		if (Input.IsKeyDown(KeyEnum.A) || Input.IsKeyDown(KeyEnum.Left))
			delta += Vector3.UnitX * -1 * velocidadX;
		if (Input.IsKeyDown(KeyEnum.D) || Input.IsKeyDown(KeyEnum.Right))
			delta += Vector3.UnitX * 1 * velocidadX;
		/*if (Input.IsKeyDown(KeyEnum.W))
			delta += Entity.Transformation.DirectionY * 1 * velocidadY;
		if (Input.IsKeyDown(KeyEnum.S))
			delta += Entity.Transformation.DirectionY * -1 * velocidadY;*/ 
			
		if (delta != Vector3.Zero)
			rotacionAcumulada += Time.DeltaTime + delta.X * Time.DeltaTime * 0.8f;
		else
			rotacionAcumulada = rotacionAcumulada - rotacionAcumulada * (5.0f * Time.DeltaTime);
		
		if (rotacionAcumulada >= 0.5f)
			rotacionAcumulada = 0.5f;
		else if (rotacionAcumulada <= -0.5f)
			rotacionAcumulada = -0.5f;
		else if (Math.Abs(rotacionAcumulada) < 0.01f)
			rotacionAcumulada = 0.0f;
			
		//Console.WriteLog(rotacionAcumulada.ToString());
			
		Transformation.LocalRotation = Quaternion.FromAxisAngle(Vector3.UnitY, rotacionAcumulada);
			
		if (delta != Vector3.Zero)
			Transformation.LocalTranslation += delta * Time.DeltaTime;
			
		if (retrasoDisparo > 0)
			retrasoDisparo -= Time.DeltaTime;
			
		if (Input.IsKeyDown(KeyEnum.Space))
			disparar();
	}
	
	int contadorDisparos;
	
	private void disparar()
	{
		if (retrasoDisparo <= 0)
		{
			Entity disparo = ComponentFactory.NewCube();
			LightSource light = new LightSource();
			light.Entity = disparo;
			light.Quadratic = true;
			
			switch((contadorDisparos++) % 3)
			{
				case 0:
					light.DiffuseColor = new Color4(1.0f, 0.0f, 0.0f, 0.0f);
					break;
				case 1:
					light.DiffuseColor = new Color4(0.0f, 1.0f, 0.0f, 0.0f);
					break;
				case 2:
					light.DiffuseColor = new Color4(0.0f, 0.0f, 1.0f, 0.0f);
					break;
			}
			
			light.Distance = 0.5f;
			
			disparo.Name = "Disparo";
			disparo.Transformation.LocalTranslation = Transformation.LocalTranslation;
			disparo.Transformation.LocalRotation = Transformation.LocalRotation;
			new MovedorDisparo().Entity = disparo;
			disparo.Transformation.LocalScale *= 0.2f;
			
			Entity entPs = ComponentFactory.NewParticleSystem();
			entPs.Transformation.Parent = disparo.Transformation;
			entPs.Transformation.LocalTranslation = Vector3.Zero;
			
			ParticleSystem ps =  (ParticleSystem) entPs.FindChildComponent(typeof(ParticleSystem));
			
			ps.MaxSize = 0.05f;
			ps.MinSize = 0.01f;
			ps.MaxParticleCount = 100;
			ps.ParticlesPerSecond = 30;
			ps.GlobalPosition = true;
			ps.Direction = new Vector3(0.0f,-1.0f,0.0f);
			ps.Spread = 30.0f;
			ps.Color = light.DiffuseColor;
			
			if (disparosPorSegundo != 0)
				retrasoDisparo = 1.0f / (float) disparosPorSegundo;
		}
	}
}

class MovedorDisparo : Script
{
	private float lifeLeft = 2;
	public override void Update()
	{
		Transformation.LocalTranslation += Transformation.DirectionY * 1 * Time.DeltaTime * 10;
		
		lifeLeft -= Time.DeltaTime;
		
		if (lifeLeft <= 0)
			Entity.Destroy();
	}
	
	public override void Collision(Collisionable col)
	{
		//Console.WriteLog("Colision!");
		Entity.Destroy();
	}
}