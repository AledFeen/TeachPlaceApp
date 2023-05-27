using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TeachPlaceApp
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void testSortMethod()
        {
            //arrange
            TableUsers expected = new TableUsers();
            TableUsers actual = new TableUsers();
            RegisteredUser reg1 = new RegisteredUser("user1", "Fewrsdf3", "+380972345467", "sfs@dsf.com");
            RegisteredUser reg2 = new RegisteredUser("user2", "Fewrsdf3", "+380972345467", "sfs@dsf.com");
            Administrator adm1 = new Administrator("admin1", "aFd2324");
            reg1.Cost = 10;
            reg2.Cost = 20;
            expected.Add(reg1);
            expected.Add(reg2);
            actual.Add(reg2);
            actual.Add(reg1);
            actual.Add(adm1);
            //act
            var actual1 = actual.selectOnlyRegistered().sort();
            //assert
            CollectionAssert.AreEqual(expected, actual1);
        }

        [TestMethod]
        public void testReverseMethod()
        {
            //arrange
            TableUsers expected = new TableUsers();
            TableUsers actual = new TableUsers();
            RegisteredUser reg1 = new RegisteredUser("user1", "Fewrsdf3", "+380972345467", "sfs@dsf.com");
            RegisteredUser reg2 = new RegisteredUser("user2", "Fewrsdf3", "+380972345467", "sfs@dsf.com");
            Administrator adm1 = new Administrator("admin1", "aFd2324");
            reg1.Cost = 10;
            reg2.Cost = 20;
            expected.Add(reg2);
            expected.Add(reg1);
            actual.Add(reg1);
            actual.Add(reg2);
            actual.Add(adm1);
            //act
            var actual1 = actual.selectOnlyRegistered().reverse();
            //assert
            CollectionAssert.AreEqual(expected, actual1);
        }
        [TestMethod]
        public void testFilterByCost()
        {
            //arrange
            TableUsers expected = new TableUsers();
            TableUsers actual = new TableUsers();
            RegisteredUser reg1 = new RegisteredUser("user1", "Fewrsdf3", "+380972345467", "sfs@dsf.com");
            RegisteredUser reg2 = new RegisteredUser("user2", "Fewrsdf3", "+380972345467", "sfs@dsf.com");
            Administrator adm1 = new Administrator("admin1", "aFd2324");
            reg1.Cost = 10;
            reg2.Cost = 20;
            expected.Add(reg1);
            //act
            actual.Add(reg2);
            actual.Add(reg1);
            actual.Add(adm1);
            var actual1 = actual.selectOnlyRegistered().filterUsers(15, 0);
            //assert
            CollectionAssert.AreEqual(expected, actual1);
        }

        [TestMethod]
        public void testFilterByName()
        {
            //arrange
            TableUsers expected = new TableUsers();
            TableUsers actual = new TableUsers();
            RegisteredUser reg1 = new RegisteredUser("user1", "Fewrsdf3", "+380972345467", "sfs@dsf.com");
            RegisteredUser reg2 = new RegisteredUser("user2", "Fewrsdf3", "+380972345467", "sfs@dsf.com");
            Administrator adm1 = new Administrator("admin1", "aFd2324");
            reg1.Name = "Володимир";
            expected.Add(reg1);
            //act
            actual.Add(reg2);
            actual.Add(reg1);
            actual.Add(adm1);
            var actual1 = actual.selectOnlyRegistered().filterUsers("Володимир");
            //assert
            CollectionAssert.AreEqual(expected, actual1);
        }

        [TestMethod]
        public void testUserExisting()
        {
            //arange
            TableUsers users = new TableUsers();
            TableUsers actual = new TableUsers();
            RegisteredUser reg1 = new RegisteredUser("user1", "Fewrsdf3", "+380972345467", "sfs@dsf.com");
            RegisteredUser reg2 = new RegisteredUser("user2", "Fewrsdf3", "+380972345467", "sfs@dsf.com");
            Administrator adm1 = new Administrator("admin1", "aFd2324");
            users.Add(reg1);
            users.Add(reg2);
            users.Add(adm1);

            //act + assert
            Assert.IsTrue(users.checkIfUserExists("user1"));
        }

        [TestMethod]
        public void isCurrectLogin()
        {
            Administrator adm = new Administrator("admin1", "aFd2324");
            Assert.ThrowsException<System.Exception>(() => adm.Login = "asd");
        }

        [TestMethod]
        public void isCurrentPassword()
        {
            Administrator adm = new Administrator("admin1", "aFd2324");
            Assert.ThrowsException<System.Exception>(() => adm.Password = "asd");
        }

        [TestMethod]
        public void isCurrentPassword1()
        {
            Administrator adm = new Administrator("admin1", "aFd2324");
            Assert.ThrowsException<System.Exception>(() => adm.Password = "asdFsad");
        }

        [TestMethod]
        public void isCurrentPassword2()
        {
            Administrator adm = new Administrator("admin1", "sdfS32f");
            string expected = "sdfS32f";

            Assert.AreEqual(expected, adm.Password);
        }

        [TestMethod]
        public void isCurrentPhone() 
        {
            RegisteredUser reg1 = new RegisteredUser("user1", "Fewrsdf3", "+380972345467", "sfs@dsf.com");
            Assert.ThrowsException<System.Exception>(() => reg1.PhoneNumber = "380972345467");
        }

        [TestMethod]
        public void isCurrentPhone1()
        {
            RegisteredUser reg1 = new RegisteredUser("user1", "Fewrsdf3", "+380972345467", "sfs@dsf.com");
            Assert.ThrowsException<System.Exception>(() => reg1.PhoneNumber = "+3809");
        }

        [TestMethod]
        public void isCurrentPhone2()
        {
            RegisteredUser reg1 = new RegisteredUser("user1", "Fewrsdf3", "+380972345467", "sfs@dsf.com");
            Assert.ThrowsException<System.Exception>(() => reg1.PhoneNumber = "+3809380972345467");
        }

        [TestMethod]
        public void isCurrentPhone3()
        {
            RegisteredUser reg1 = new RegisteredUser("user1", "Fewrsdf3", "+380972345467", "sfs@dsf.com");
            string expected = "+380972345467";

            Assert.AreEqual(expected, reg1.PhoneNumber);
        }

        [TestMethod]
        public void isCurrentEmail() 
        {
            RegisteredUser reg1 = new RegisteredUser("user1", "Fewrsdf3", "+380972345467", "sfs@dsf.com");

            Assert.ThrowsException<System.Exception>(() => reg1.Email = "123");
        }

        [TestMethod]
        public void isCurrentEmail1()
        {
            RegisteredUser reg1 = new RegisteredUser("user1", "Fewrsdf3", "+380972345467", "sfs@dsf.com");

            Assert.ThrowsException<System.Exception>(() => reg1.Email = "sfsdsf.com");
        }

        [TestMethod]
        public void isCurrentEmail2()
        {
            RegisteredUser reg1 = new RegisteredUser("user1", "Fewrsdf3", "+380972345467", "sfs@dsf.com");

            Assert.ThrowsException<System.Exception>(() => reg1.Email = "sfsd@sf.c");
        }

        [TestMethod]
        public void isCurrentEmail3()
        {
            RegisteredUser reg1 = new RegisteredUser("user1", "Fewrsdf3", "+380972345467", "sfs@dsf.com");

            Assert.ThrowsException<System.Exception>(() => reg1.Email = "d@s.com");
        }

        [TestMethod]
        public void isCurrentEmail4()
        {
            RegisteredUser reg1 = new RegisteredUser("user1", "Fewrsdf3", "+380972345467", "sfs@dsf.com");

            string expected = "sfs@dsf.com";

            Assert.AreEqual(expected, reg1.Email);
        }

        [TestMethod]
        public void isCurrentUsername()
        {
            Request req = new Request();

            Assert.ThrowsException<System.Exception>(() => req.User = "d@");
        }

        [TestMethod]
        public void isCurrentUsername1()
        {
            Request req = new Request();

            Assert.ThrowsException<System.Exception>(() => req.User = "dsf2");
        }

        [TestMethod]
        public void isCurrentMessage() 
        {
            Request req = new Request();

            Assert.ThrowsException<System.Exception>(() => req.Message = "dsf2");
        }

        [TestMethod]
        public void isCurrentMessage1()
        {
            Request req = new Request();

            Assert.ThrowsException<System.Exception>(() => req.Message = "dsf223dsf");
        }

        [TestMethod]
        public void isCurrentUsername2()
        {
            Messages mes = new Messages();

            Assert.ThrowsException<System.Exception>(() => mes.User = "dsf2");
        }

        [TestMethod]
        public void isCurrentMessage3()
        {
            Messages mes = new Messages();

            Assert.ThrowsException<System.Exception>(() => mes.Message = "dsf2");
        }

        [TestMethod]
        public void isCurrentMessage4()
        {
            Messages mes = new Messages();

            Assert.ThrowsException<System.Exception>(() => mes.Message = "dsf223dsf");
        }
    }
}
