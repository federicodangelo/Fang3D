namespace Fang3D
{
    #region KeyEnum

    public enum KeyEnum
    {
        None,
        Left,
        Right,
        Up,
        Down,
        Space,
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z,
        Number0,
        Number1,
        Number2,
        Number3,
        Number4,
        Number5,
        Number6,
        Number7,
        Number8,
        Number9,
        Length
    }

    #endregion

    static public class Input
    {
        static private bool[] keys = new bool[(int) KeyEnum.Length];

        static public void ProcessKeyPress(KeyEnum key, bool press)
        {
            keys[(int) key] = press;
        }

        static public bool IsKeyDown(KeyEnum key)
        {
            return keys[(int) key];
        }
    }
}
