using OpenTK.Math;
using Fang3D;

public class MovedorNaves : Script
{
	private float dir = 1.0f;
	private float dx;
	private float xInicial;
	
	public float max = 7.0f;
	public float vel = 5.0f;
	
	public override void Start()
	{
		xInicial = Transformation.LocalTranslation.X;
	}
	
	public override void Update()
	{
		dx += Time.DeltaTime * vel * dir;
		
		if (dir > 0 && dx > max)
			dir = -dir;
		else if (dir < 0 && dx < -max)
			dir = -dir;
		
		Transformation.LocalTranslation	 = new Vector3(
												dx + xInicial,
												Transformation.LocalTranslation.Y,
												Transformation.LocalTranslation.Z);
	}
}

