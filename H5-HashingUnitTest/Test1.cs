namespace H5_HashingUnitTest
{
    [TestClass]
    public sealed class Test1
    {


        [TestMethod]
        public void TestAfLogin1()
        {
            // Arrange: Create a temporary user file
            string tempFilePath = Path.GetTempFileName();
            File.WriteAllText(tempFilePath, $"HashTest2:{H5_Hashing.Program.HashPassword("Test1234")}");

            // Act
            bool result = H5_Hashing.Program.TestLogin("HashTest2", "Test1234", tempFilePath);

            // Cleanup
            File.Delete(tempFilePath);

            // Assert
            Assert.AreEqual(true, result, "Login failed");
        }

        [TestMethod]
        public void TestAfLogin2()
        {
            // Arrange: Create a temporary user file
            string tempFilePath = Path.GetTempFileName();
            File.WriteAllText(tempFilePath, $"HashTest2:{H5_Hashing.Program.HashPassword("Test1234")}");

            // Act
            bool result = H5_Hashing.Program.TestLogin("HashTest2", "TestIkke", tempFilePath);

            // Cleanup
            File.Delete(tempFilePath);

            // Assert
            Assert.AreEqual(false, result, "Login failed");
        }


    }
}
