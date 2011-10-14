﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pung
{

    class BlockManager : GameObject
    {
        // List of all blocks spawned in the game. Available to every object.
        static List<Block> blocksList;

        // Block constant.
        const int MAX_BLOCK_SIZE = 32;
        const int MIN_BLOCK_SIZE = 4;
        const double TIME_UNTIL_BLOCK_DEATH = 30;

        Rectangle blocksSafeZone;


        // Graphical components
        /* BlockBunch is given a texture because not all blocks will be carrying a texture.
         * Instead BlockBunch carries the texture and is applied to each children.
         */
        Texture2D blockTexture;

        #region Properties

        public static List<Block> Blocks
        {
            get { return BlockManager.blocksList; }
            set { BlockManager.blocksList = value; }
        }

        public Rectangle BlocksSafeZone
        {
            get { return blocksSafeZone; }
            set { blocksSafeZone = value; }
        }

        public int MaxBlockSize
        {
            get { return MAX_BLOCK_SIZE; }
        }

        public int MinBlockSize
        {
            get { return MIN_BLOCK_SIZE; }
        } 

        #endregion

        public BlockManager(PungGame game)
            : base(game)
        {
            blocksList = new List<Block>();
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Block item in blocksList)
            {// Draws each block
                item.Draw(gameTime);
            }
        }

        public override void Initialize()
        {

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // BlockBunch cannot call his Base.Update because it updates the ObjectRectangle using the texture size. Which this object lack.
            // Solution : Make Blockbunch inherits from something else
            //base.Update(gameTime);

            foreach (Block item in blocksList)
            {
                item.Update(gameTime);
            }

            List<Block> blocksToDelete = new List<Block>();

            foreach (Block item in blocksList)
            {
                if (item.LivingTime >= TIME_UNTIL_BLOCK_DEATH)
                {
                    blocksToDelete.Add(item);
                }
            }

            foreach (Block item in blocksToDelete)
            {
                blocksList.Remove(item);
            }

        }

        public  void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            blockTexture = theContentManager.Load<Texture2D>(theAssetName);

        }


        public void addBlock(PungGame game)
        {
            Block newBlock = new Block(game, blocksSafeZone);

            addBlock(newBlock, Utilities.Randomizer.CreateRandom(MIN_BLOCK_SIZE, MAX_BLOCK_SIZE),
                Utilities.Randomizer.CreateRandom(MIN_BLOCK_SIZE, MAX_BLOCK_SIZE),
                new Color(255, 0, 0));

        }

        /// <summary>
        /// Add a block into the game.
        /// </summary>
        /// <param name="newBlock">Block to be added to the game.</param>
        /// <remarks>
        /// This procedure should be re-written since the group/grouplist structures 
        /// will be reworked.
        /// </remarks>
        public void addBlock(Block newBlock)
        {

            addBlock(newBlock, 16, 16, new Color(255, 255, 255));

        }


        public void addBlock(Block newBlock, int width, int height, Color blockColor)
        {
            /* Get the group list service and get the group named Collisions.
             * take the new block and load it's texture then put it in the BlockBunch group then 
             * add the newBlock to the collisionGroup
             */
            GroupList groupList = (GroupList)Game.Services.GetService(typeof(GroupList));
            Group collisionGroup = groupList.GetGroup("Collisions");
            newBlock.Texture = Utilities.ColorTexture.Create(Game.GraphicsDevice, width, height, blockColor);
            newBlock.ObjectRectangle = new Rectangle((int)newBlock.Position.X, (int)newBlock.Position.Y, newBlock.Texture.Width, newBlock.Texture.Height);
            blocksList.Add(newBlock);
            collisionGroup.Add(newBlock);

        }

        /// <summary>
        /// Remove a block from the game.
        /// </summary>
        /// <param name="targetBlock">The block to be removed.</param>
        public void removeBlock(Block targetBlock)
        {
            // Get the groups the same way as addBlock but uses those objects to remove a block from the collections.
            GroupList groupList = (GroupList)Game.Services.GetService(typeof(GroupList));
            Group collisionGroup = groupList.GetGroup("Collisions");
            collisionGroup.Remove(targetBlock);
            blocksList.Remove(targetBlock);

        }


    }
}
