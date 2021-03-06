﻿using System;

namespace TrabalhoCG3 {
    /// <summary>
    /// Classe responsável pela matriz de transformação
    /// </summary>
    public class Transform {
        #region Fields
        public const double DEG_TO_RAD = 0.017453292519943295769236907684886;
        #endregion
        #region Methods
        public Transform() {
            
        }
        private static Transform MatrixTranslate = new Transform();
        private static Transform MatrixInverseTranslate = new Transform();

        private double[] matrix =
        {    1, 0, 0, 0 ,
             0, 1, 0, 0 ,
             0, 0, 1, 0 ,
             0, 0, 0, 1     };

        public double[] Matrix {
            set {
                for(int i = 0; i < matrix.Length; i++) {
                    matrix[i] = value[i];
                } }
            get { return matrix; }
        }
        /// <summary>
        /// Coloca um valor em um determinado local na matriz
        /// </summary>
        /// <param name="valor">Valor.</param>
        /// <param name="indice">Indice.</param>
        public void setMatrix(double valor, int indice){
            matrix[indice] = valor;
        }
        /// <summary>
        /// Coloca a matriz identidade no lugar
        /// </summary>
        public void SetIdentity() {
            for (int i=0; i<16; ++i) {
                matrix[i] = 0.0;
            }
            matrix[0] = matrix[5] = matrix[10] = matrix[15] = 1.0;
        }
        /// <summary>
        /// Faz a translação da matriz
        /// </summary>
        /// <param name="tx">X.</param>
        /// <param name="ty">Y.</param>
        /// <param name="tz">Z.</param>
        public void SetTranslation(double tx, double ty, double tz)
        {
            SetIdentity();
            matrix[12] = tx;
            matrix[13] = ty;
            matrix[14] = tz;
        }
        /// <summary>
        /// Faz a translação da matriz (enquanto estiver ainda tranladando)
        /// </summary>
        /// <param name="tx">X.</param>
        /// <param name="ty">Y.</param>
        /// <param name="tz">Z.</param>
        public void AddTranslation(double tx, double ty, double tz)
        {
            //AddIdentity();
            matrix[12] += tx;
            matrix[13] += ty;
            matrix[14] += tz;
        }
        /// <summary>
        /// Faz a escala da matriz
        /// </summary>
        /// <param name="sX">X.</param>
        /// <param name="sY">Y.</param>
        /// <param name="sZ">Z.</param>
        /// <param name="center">Referência.</param>
        public void SetScale(double sX, double sY, double sZ, Point4D center)
        {
            Transform.MatrixInverseTranslate.SetTranslation(-center.X, -center.Y, -center.Z);
            Transform.MatrixTranslate.SetTranslation(center.X, center.Y, center.Z);

            SetIdentity();
            Matrix = TransformMatrix(Transform.MatrixTranslate).Matrix;

            matrix[0] =  sX;
            matrix[5] =  sY;
            matrix[10] = sZ;

            Matrix = TransformMatrix(Transform.MatrixInverseTranslate).Matrix;
        }
        /// <summary>
        /// Faz a rotação da matriz no eixo X
        /// </summary>
        /// <param name="radians">Ângulol.</param>
        /// <param name="center">Referência.</param>
        public void SetRotationX(double radians, Point4D center)
        {
            Transform.MatrixInverseTranslate.SetTranslation(-center.X, -center.Y, -center.Z);
            Transform.MatrixTranslate.SetTranslation(center.X, center.Y, center.Z);

            SetIdentity();
            Matrix = TransformMatrix(Transform.MatrixTranslate).Matrix;

            matrix[5] =   Math.Cos(radians);
            matrix[9] =  -Math.Sin(radians);
            matrix[6] =   Math.Sin(radians);
            matrix[10] =  Math.Cos(radians);

            Matrix = TransformMatrix(Transform.MatrixInverseTranslate).Matrix;
        }
        /// <summary>
        /// Faz a rotação da matriz no eixo Y
        /// </summary>
        /// <param name="radians">Ângulol.</param>
        /// <param name="center">Referência.</param>
        public void SetRotationY(double radians, Point4D center)
        {
            Transform.MatrixInverseTranslate.SetTranslation(-center.X, -center.Y, -center.Z);
            Transform.MatrixTranslate.SetTranslation(center.X, center.Y, center.Z);

            SetIdentity();
            Matrix = TransformMatrix(Transform.MatrixTranslate).Matrix;

            matrix[0] =   Math.Cos(radians);
            matrix[8] =   Math.Sin(radians);
            matrix[2] =  -Math.Sin(radians);
            matrix[10] =  Math.Cos(radians);

            Matrix = TransformMatrix(Transform.MatrixInverseTranslate).Matrix;
        }
        /// <summary>
        /// Faz a rotação da matriz no eixo Z
        /// </summary>
        /// <param name="radians">Ângulol.</param>
        /// <param name="center">Referência.</param>
        public void SetRotationZ(double radians, Point4D center)
        {
            Transform.MatrixInverseTranslate.SetTranslation(-center.X, -center.Y, -center.Z);
            Transform.MatrixTranslate.SetTranslation(center.X, center.Y, center.Z);

            SetIdentity();
            Matrix = TransformMatrix(Transform.MatrixTranslate).Matrix;

            matrix[0] =  Math.Cos(radians);
            matrix[4] = -Math.Sin(radians);
            matrix[1] =  Math.Sin(radians);
            matrix[5] =  Math.Cos(radians);


            Matrix = TransformMatrix(Transform.MatrixInverseTranslate).Matrix;
        }

        /// <summary>
        /// Faz a transformação da matriz em um determinado ponto
        /// </summary>
        /// <returns>O ponto a ser transformado</returns>
        /// <param name="point">O ponto transformado</param>
        public Point4D TransformPoint(Point4D point) {
            Point4D pointResult = new Point4D(
                matrix[0]*point.X  + matrix[4]*point.Y + matrix[8]*point.Z + matrix[12]*point.W,
                matrix[1]*point.X  + matrix[5]*point.Y + matrix[9]*point.Z + matrix[13]*point.W,
                matrix[2]*point.X  + matrix[6]*point.Y + matrix[10]*point.Z + matrix[14]*point.W,
                matrix[3]*point.X  + matrix[7]*point.Y + matrix[11]*point.Z + matrix[15]*point.W
            );
            return pointResult;
        }
        /// <summary>
        /// Faz a multiplicação das matrizes de transfomação
        /// </summary>
        /// <returns>A matriz a ser transformada</returns>
        /// <param name="t">A matriz transformada</param>
        public Transform TransformMatrix(Transform t) {
            Transform result = new Transform();
            for (int i=0; i < 16; ++i)
                result.matrix[i] =
                    matrix[i%4]*t.matrix[i/4*4]+matrix[(i%4)+4]*t.matrix[i/4*4+1]
                    + matrix[(i%4)+8]*t.matrix[i/4*4+2]+matrix[(i%4)+12]*t.matrix[i/4*4+3];
            return result;
        }

        public override string ToString() {
            string res = "";
            res += string.Format("|{0},{1},{2},{3}|", Matrix[0],Matrix[1],Matrix[2],Matrix[3]);
            res += string.Format("|{0},{1},{2},{3}|", Matrix[4],Matrix[5],Matrix[6],Matrix[7]);
            res += string.Format("|{0},{1},{2},{3}|", Matrix[8],Matrix[9],Matrix[10],Matrix[11]);
            res += string.Format("|{0},{1},{2},{3}|", Matrix[12],Matrix[13],Matrix[14],Matrix[15]);
            return res;

        }
        #endregion
    }
}

