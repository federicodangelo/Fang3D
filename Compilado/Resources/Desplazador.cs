using OpenTK.Math;
using Fang3D;

public class Desplazador : Script
{
	public override void Update()
	{
		Entity.Transformation.LocalTranslation	 += Vector3.UnitX * 2 * Time.DeltaTime;
	
		/*if (rotarAutomaticamente)
		{
			Entity.Transformation.LocalRotation *=
				Quaternion.FromAxisAngle(Vector3.UnitY, Functions.DegreesToRadians(velocidadRotacion.Y * Time.DeltaTime)) *
				Quaternion.FromAxisAngle(Vector3.UnitX, Functions.DegreesToRadians(velocidadRotacion.X * Time.DeltaTime)) *
				Quaternion.FromAxisAngle(Vector3.UnitZ, Functions.DegreesToRadians(velocidadRotacion.Z * Time.DeltaTime));
		}*/
	}
}

