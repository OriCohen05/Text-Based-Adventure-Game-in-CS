using System;
using System.Collections.Generic;
using System.IO;

namespace TextAdventureGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Mysterious Dungeon!");

            GameWorld gameWorld = new GameWorld();
            gameWorld.Play();

            Console.WriteLine("Thanks for playing!");
        }
    }

    class GameObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    class Room : GameObject
    {
        public List<string> InteractableObjects { get; set; }
        public List<Enemy> Enemies { get; set; }
        public Dictionary<string, Room> ConnectedRooms { get; set; }

        public Room(string name, string description)
        {
            Name = name;
            Description = description;
            InteractableObjects = new List<string>();
            Enemies = new List<Enemy>();
            ConnectedRooms = new Dictionary<string, Room>();
        }
    }

    class Enemy : GameObject
    {
        public int Health { get; set; }

        public Enemy(string name, string description, int health)
        {
            Name = name;
            Description = description;
            Health = health;
        }
    }

    class Player
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public Room CurrentRoom { get; set; }

        public Player(string name, int health, Room startingRoom)
        {
            Name = name;
            Health = health;
            CurrentRoom = startingRoom;
        }
    }

    class GameWorld
    {
        private Player player;
        private bool gameOver;

        public void Play()
        {
            InitializeGame();

            while (!gameOver)
            {
                DisplayCurrentRoom();
                ProcessPlayerInput();
            }
        }

        private void InitializeGame()
        {
            // Initialize rooms, enemies, and connections here

            // Set the starting room
            Room startingRoom = new Room("Starting Room", "You find yourself in a dimly lit room.");
            player = new Player("Player", 100, startingRoom);
        }

        private void DisplayCurrentRoom()
        {
            Console.WriteLine($"You are in the {player.CurrentRoom.Name}.");
            Console.WriteLine(player.CurrentRoom.Description);
            Console.WriteLine("You see the following objects in the room:");
            foreach (var obj in player.CurrentRoom.InteractableObjects)
            {
                Console.WriteLine(obj);
            }
            Console.WriteLine("You encounter the following enemies:");
            foreach (var enemy in player.CurrentRoom.Enemies)
            {
                Console.WriteLine($"{enemy.Name} (Health: {enemy.Health})");
            }
            Console.WriteLine("You can go to the following rooms:");
            foreach (var roomName in player.CurrentRoom.ConnectedRooms.Keys)
            {
                Console.WriteLine(roomName);
            }
        }

        private void ProcessPlayerInput()
        {
            Console.Write("Enter your command: ");
            string input = Console.ReadLine().ToLower();
            string[] inputParts = input.Split(' ');

            if (inputParts[0] == "go" && inputParts.Length > 1)
            {
                if (player.CurrentRoom.ConnectedRooms.ContainsKey(inputParts[1]))
                {
                    player.CurrentRoom = player.CurrentRoom.ConnectedRooms[inputParts[1]];
                }
                else
                {
                    Console.WriteLine("You can't go there.");
                }
            }
            else if (inputParts[0] == "save")
            {
                SaveGame();
            }
            else if (inputParts[0] == "load")
            {
                LoadGame();
            }
            else if (inputParts[0] == "quit")
            {
                gameOver = true;
            }
            else
            {
                Console.WriteLine("Invalid command.");
            }
        }

        private void SaveGame()
        {
            using (StreamWriter writer = new StreamWriter("save.txt"))
            {
                writer.WriteLine(player.Name);
                writer.WriteLine(player.Health);
                writer.WriteLine(player.CurrentRoom.Name);
            }
            Console.WriteLine("Game saved.");
        }

        private void LoadGame()
        {
            if (File.Exists("save.txt"))
            {
                using (StreamReader reader = new StreamReader("save.txt"))
                {
                    player.Name = reader.ReadLine();
                    player.Health = int.Parse(reader.ReadLine());
                    string currentRoomName = reader.ReadLine();
                    // Find the room with the given name and set it as the player's current room
                    // (You need to implement a method to find the room by its name)
                }
                Console.WriteLine("Game loaded.");
            }
            else
            {
                Console.WriteLine("No saved game found.");
            }
        }
    }
}
