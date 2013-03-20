using OpenTK.Math;
using Fang3D;

public class Rotador : Script
{
	public bool rotarAutomaticamente = false;

	private Vector3 velocidadRotacion;

	public Vector3 VelocidadRotacion
	{
		get { return velocidadRotacion; }
		set { velocidadRotacion = value; }
	}

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

