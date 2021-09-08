﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP;
using TGC.MonoGame.TP.Ships;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Esta es la clase principal  del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class TGCGame : Game
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";
        
        private FollowCamera FollowCamera { get; set; }
        private FreeCamera FreeCamera { get; set; }
        private Camera Camera { get; set; }

        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);
            // Descomentar para que el juego sea pantalla completa.
            Graphics.IsFullScreen = true;
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

        private GraphicsDeviceManager Graphics { get; }

        private int naves = 400;

        private ShipA[] shipsA;
        private ShipB[] shipsB;

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

            // Configuro el tamaño de la pantalla
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.ApplyChanges();
            // Creo una camara para seguir a nuestro auto

            FollowCamera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);
            FreeCamera = new FreeCamera(GraphicsDevice, this.Window);

            Camera = new Camera(60, GraphicsDevice,0.1f,1000f);

            shipsA = new ShipA[naves];
            shipsB = new ShipB[naves];
            for (int i = 0; i < naves*2; i++)
            {
                var variacion = 100;
                var rand = new Random();

                if(i < naves)
                {
                    shipsA[i] = new ShipA(Content);
                    shipsA[i].Position.Z = ((i % 20) * 300) + rand.Next(-variacion, variacion);
                    shipsA[i].Position.X = ((int)Math.Floor(i / 20f) * 500) + rand.Next(-variacion * 2, variacion * 2);
                }
                else
                {
                    shipsB[i - naves] = new ShipB(Content);
                    shipsB[i - naves].Position.Z = ((i % 20) * 300) + rand.Next(-variacion, variacion);
                    shipsB[i - naves].Position.X = ((int)Math.Floor(i / 20f) * 500) + rand.Next(-variacion * 2, variacion * 2);
                }

            }


            base.Initialize();
        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
        ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el procesamiento
        ///     que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void LoadContent()
        {
            for (int i = 0; i < naves; i++)
            {
                shipsA[i].Load();
            }
            for (int i = 0; i < naves; i++)
            {
                shipsB[i].Load();
            }

            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de actualizacion del juego.

            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Salgo del juego.
                Exit();

            // Basado en el tiempo que paso se va generando una rotacion.
            
            for (int i = 0; i < naves; i++)
            {
                shipsA[i].Update(gameTime);
            }
            for (int i = 0; i < naves; i++)
            {
                shipsB[i].Update(gameTime);
            }
            if(Keyboard.GetState().IsKeyDown(Keys.Enter)){
                FreeCamera = new FreeCamera(GraphicsDevice, this.Window);
            }
            Camera.Update(gameTime);
            FreeCamera.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.Black);

            for (int i = 0; i < naves; i++)
            {
                shipsA[i].Draw(FreeCamera.View,FreeCamera.Projection,Color.White);
            }
             
            for (int i = 0; i < naves; i++)
            {
                shipsB[i].Draw(FreeCamera.View, FreeCamera.Projection, Color.Blue);
            }

            base.Draw(gameTime);
        }

        /// <summary>
        ///     Libero los recursos que se cargaron en el juego.
        /// </summary>
        protected override void UnloadContent()
        {
            // Libero los recursos.
            Content.Unload();

            base.UnloadContent();
        }
    }
}