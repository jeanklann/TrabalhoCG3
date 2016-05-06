﻿using System;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace TrabalhoCG3 {
    /// <summary>
    /// Classe MAIN
    /// </summary>
    public class Trabalho3 {
        public static void Main(string[] args){
            States.World = new World();

            Game game = new Game();

            game.ClientSize = new Size (400, 400);
            game.Run (60);
        }
    }
}

