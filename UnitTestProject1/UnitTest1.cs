using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace laba3
{
    [TestClass]
    public class AllTests
    {
        [Serializable]
        public class Choice
        {
            public string Description { get; set; }
            public int NextRoom { get; set; }

            public Choice(string description, int nextRoom)
            {
                Description = description;
                NextRoom = nextRoom;
            }
        }

        [Serializable]
        public class Player
        {
            public string Name { get; set; }
            public int CurrentRoom { get; set; }

            public Player(string name, int currentRoom)
            {
                Name = name;
                CurrentRoom = currentRoom;
            }
        }

        [Serializable]
        public class Room
        {
            public string Description { get; set; }
            public List<Choice> Choices { get; set; }
            public bool IsSolved { get; set; }

            // Оставлен только первоначальный конструктор
            public Room(string description)
            {
                Description = description;
                Choices = new List<Choice>();
                IsSolved = false;
            }

            public void AddChoice(string description, int nextRoom)
            {
                Choices.Add(new Choice(description, nextRoom));
            }
        }

        [Serializable]
        public class Game
        {
            public Player Player { get; set; }
            public Room[] Rooms { get; set; }

            public Game(Player player, Room[] rooms)
            {
                Player = player;
                Rooms = rooms;
            }
        }

        [TestMethod]
        public void ChoiceConstructor_InitializedCorrectly()
        {
            // Arrange
            string description = "Test Description";
            int nextRoom = 42;

            // Act
            Choice choice = new Choice(description, nextRoom);

            // Assert
            Assert.AreEqual(description, choice.Description);
            Assert.AreEqual(nextRoom, choice.NextRoom);
        }

        [TestMethod]
        public void PlayerConstructor_InitializedCorrectly()
        {
            // Arrange
            string playerName = "TestPlayer";
            int currentRoom = 1;

            // Act
            Player player = new Player(playerName, currentRoom);

            // Assert
            Assert.AreEqual(playerName, player.Name);
            Assert.AreEqual(currentRoom, player.CurrentRoom);
        }

        [TestMethod]
        public void GameConstructor_InitializedCorrectly()
        {
            // Arrange
            Player player = new Player("TestPlayer", 1);
            Room[] rooms = new Room[] { new Room("TestRoom") };

            // Act
            Game game = new Game(player, rooms);

            // Assert
            Assert.AreEqual(player, game.Player);
            CollectionAssert.AreEqual(rooms, game.Rooms);
        }

        [TestMethod]
        public void RoomConstructor_InitializedCorrectly()
        {
            // Arrange
            string roomDescription = "Test Room Description";

            // Act
            Room room = new Room(roomDescription);

            // Assert
            Assert.AreEqual(roomDescription, room.Description);
            Assert.IsNotNull(room.Choices);
            Assert.IsFalse(room.IsSolved);
            Assert.AreEqual(0, room.Choices.Count);
        }

        [TestMethod]
        private void AssertEqualGames(Game expected, Game actual)
        {
            Assert.AreEqual(expected.Player.Name, actual.Player.Name);
            Assert.AreEqual(expected.Player.CurrentRoom, actual.Player.CurrentRoom);

            Assert.AreEqual(expected.Rooms.Length, actual.Rooms.Length);

            for (int i = 0; i < expected.Rooms.Length; i++)
            {
                Assert.AreEqual(expected.Rooms[i].Description, actual.Rooms[i].Description);
                Assert.AreEqual(expected.Rooms[i].IsSolved, actual.Rooms[i].IsSolved);

                // Сравниваем Choices поэлементно
                Assert.AreEqual(expected.Rooms[i].Choices.Count, actual.Rooms[i].Choices.Count);

                for (int j = 0; j < expected.Rooms[i].Choices.Count; j++)
                {
                    Assert.AreEqual(expected.Rooms[i].Choices[j].Description, actual.Rooms[i].Choices[j].Description);
                    Assert.AreEqual(expected.Rooms[i].Choices[j].NextRoom, actual.Rooms[i].Choices[j].NextRoom);
                }
            }
        }

        [TestMethod]
        public void SaveAndLoadGame_ShouldWorkCorrectly()
        {
            // Arrange
            Player player = new Player("TestPlayer", 1);

            Room[] rooms = {new Room("Test Room 1"), new Room("Test Room 2")};

            rooms[0].AddChoice("Test Choice 1", 2);
            rooms[1].AddChoice("Test Choice 2", 1);

            Game originalGame = new Game(player, rooms);

            // Act
            SaveAndLoadGame(originalGame, out Game loadedGame);

            // Assert
            AssertEqualGames(originalGame, loadedGame);
        }

        private void SaveAndLoadGame(Game originalGame, out Game loadedGame)
        {
            // Save
            SaveGame(originalGame);

            // Load
            LoadGame(out loadedGame);
        }

        private void SaveGame(Game game)
        {
            try
            {
                using (FileStream fs = new FileStream("savegame_test.dat", FileMode.Create))
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, game);
                }
            }
            catch (SerializationException ex)
            {
                Assert.Fail("Serialization failed: " + ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to save the game: " + ex.Message);
            }
        }

        private void LoadGame(out Game game)
        {
            game = null;

            try
            {
                using (FileStream fs = new FileStream("savegame_test.dat", FileMode.Open))
                {
                    IFormatter formatter = new BinaryFormatter();
                    game = (Game)formatter.Deserialize(fs);
                }
            }
            catch (SerializationException ex)
            {
                Assert.Fail("Deserialization failed: " + ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to load the game: " + ex.Message);
            }
        }
    }
}
