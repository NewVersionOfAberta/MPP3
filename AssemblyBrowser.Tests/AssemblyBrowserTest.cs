using System;
using System.IO;
using AssemblyBrowserLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssemblyBrowser.Tests
{
    [TestClass]
    public class AssemblyBrowserTest
    {
        const string PATH_TO_LIB = "E:/University_needs/СПП/lab3/TestLib/bin/Debug/netstandard2.0/TestLib.dll";
        TreeCreator treeCreator;

        [TestInitialize]
        public void InitializeTest()
        {
            treeCreator = new TreeCreator();
        }

        [TestMethod]
        public void TestBuildTree_1lvlNode_1()
        {
            int expected = 1;

            var actual = treeCreator.BuildTree(PATH_TO_LIB).Count; 
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void TestBuildTree_2lvlNode_2()
        {
            int expected = 2;

            var actual = treeCreator.BuildTree(PATH_TO_LIB)[0].SubNodes.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestBuildTree_2lvlNodeTypeClass_2()
        {
            int expected = 2;
            int actual = 0;
            var nodes = treeCreator.BuildTree(PATH_TO_LIB)[0].SubNodes;
            foreach (var node in nodes)
            {
                if (node.Type == Types.Class)
                {
                    actual++;
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestBuildTree_3lvlNodeTypeMethod_1()
        {
            int expected = 1;
            int actual = 0;

            var nodes = treeCreator.BuildTree(PATH_TO_LIB)[0].SubNodes[0].SubNodes;

            foreach (var node in nodes)
            {
                if (node.Type == Types.Method)
                {
                    actual++;
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestBuildTree_3lvlNodeTypeFields_3()
        {
            int expected = 3;
            int actual = 0;

            var nodes = treeCreator.BuildTree(PATH_TO_LIB)[0].SubNodes[0].SubNodes;

            foreach (var node in nodes)
            {
                if (node.Type == Types.Field)
                {
                    actual++;
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestBuildTree_3lvlNodeTypeConstructors_2()
        {
            int expected = 2;
            int actual = 0;

            var nodes = treeCreator.BuildTree(PATH_TO_LIB)[0].SubNodes[0].SubNodes;

            foreach (var node in nodes)
            {
                if (node.Type == Types.Constructor)
                {
                    actual++;
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestBuildTree_3lvlNodeTypeProperty_1()
        {
            int expected = 1;
            int actual = 0;

            var nodes = treeCreator.BuildTree(PATH_TO_LIB)[0].SubNodes[0].SubNodes;

            foreach (var node in nodes)
            {
                if (node.Type == Types.Property)
                {
                    actual++;
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestBuildTree_EmptyPath_FileNotFoundException()
        {
            Assert.ThrowsException<ArgumentException>(() => treeCreator.BuildTree(""));
        }

        [TestMethod]
        public void TestBuildTree_InvalidPath_NotSupportedException()
        {
            Assert.ThrowsException<NotSupportedException>(() => treeCreator.BuildTree(
                @"C:\Windows\Microsoft.NET\assembly\GAC_64\mscorlib\v4.0_4.0.0.0__b77a5c561934e089\mscorlib.dll"));
        }

    }
}
