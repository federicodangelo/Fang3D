using OpenTK.Math;
using Fang3D;

public class Rotador2 : Script
{
	public bool rotarAutomaticamente = true;

	public Vector3 velocidadRotacion = new Vector3(180, 180, 180);

	public override void Update()
	{
		if (rotarAutomaticamente)
		{
			Entity.Transformation.LocalRotation *=
				Quaternion.FromAxisAngle(Vector3.UnitY, Functions.DegreesToRadians(velocidadRotacion.Y * Time.DeltaTime)) *
				Quaternion.FromAxisAngle(Vector3.UnitX, Functions.DegreesToRadians(velocidadRotacion.X * Time.DeltaTime)) *
				Quaternion.FromAxisAngle(Vector3.UnitZ, Functions.DegreesToRadians(velocidadRotacion.Z * Time.DeltaTime));
		}
	}
}

