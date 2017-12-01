namespace Ancestry.Daisy.Tests.Daisy.Unit.Linking
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Language.AST;
    using Ancestry.Daisy.Linking;
    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.TestObjects;

    using Moq;

    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture,Category("Unit")]
    public class DaisyLinkerTest
    {
        /// <summary>
        /// Its the links statements.
        /// </summary>
        [Test]
        public void ItLinksStatements()
        {
            var linkedStatement = new Mock<ILinkedStatement>();
            var statement = new Mock<IStatementDefinition>();
            statement.Setup(x => x.Link(It.IsAny<string>())).Returns(linkedStatement.Object);
            statement.SetupGet(x => x.Name).Returns("Tennant");
            statement.SetupGet(x => x.ScopeType).Returns(typeof(Int32));
            var statementSet = new StatementSet().Add(statement.Object);

            var ast = new DaisyAst(new StatementNode("Hello gov'nor"));

            var load = new DaisyLinker(ast,statementSet,typeof(int));

            load.Link();
            statement.Verify(x => x.Link(It.IsAny<string>()), Times.Once());
        }

        /// <summary>
        /// Its the dies on failure to link.
        /// </summary>
        [Test]
        public void ItDiesOnFailureToLink()
        {
            var statementSet = new StatementSet();
            var ast = new DaisyAst(new StatementNode("Hello gov'nor"));
            var load = new DaisyLinker(ast,statementSet,typeof(int));
            var ex = Assert.Catch<FailedLinkException>(() => load.Link());
            Assert.AreEqual(1, ex.Errors.Count);
            Assert.IsInstanceOf<NoLinksFoundError>(ex.Errors.First());
        }

        /// <summary>
        /// Its the dies on multiple links found.
        /// </summary>
        [Test]
        public void ItDiesOnMultipleLinksFound()
        {
            var linkedStatement1 = new Mock<ILinkedStatement>();
            var statement = new Mock<IStatementDefinition>();
            linkedStatement1.SetupGet(x => x.Definition).Returns(statement.Object);
            statement.Setup(x => x.Link(It.IsAny<string>())).Returns(linkedStatement1.Object);
            statement.SetupGet(x => x.Name).Returns("David");
            statement.SetupGet(x => x.ScopeType).Returns(typeof(int));

            var linkedStatement2 = new Mock<ILinkedStatement>();
            var statement2 = new Mock<IStatementDefinition>();
            linkedStatement2.SetupGet(x => x.Definition).Returns(statement2.Object);
            statement2.Setup(x => x.Link(It.IsAny<string>())).Returns(linkedStatement2.Object);
            statement2.SetupGet(x => x.Name).Returns("Tennant");
            statement2.SetupGet(x => x.ScopeType).Returns(typeof(int));
            var statementSet = new StatementSet().Add(statement.Object).Add(statement2.Object);

            var ast = new DaisyAst(new StatementNode("Hello gov'nor"));

            var load = new DaisyLinker(ast,statementSet,typeof(int));
            var ex = Assert.Catch<FailedLinkException>(() => load.Link());
            Assert.AreEqual(1, ex.Errors.Count);
            Assert.IsInstanceOf<MultipleLinksFoundError>(ex.Errors.First());
        }

        /// <summary>
        /// Its the dies on linking group to non group.
        /// </summary>
        [Test]
        public void ItDiesOnLinkingGroupToNonGroup()
        {
            var linkedStatement1 = new Mock<ILinkedStatement>();
            var statement = new Mock<IStatementDefinition>();
            linkedStatement1.SetupGet(x => x.Definition).Returns(statement.Object);
            statement.Setup(x => x.Link(It.IsAny<string>())).Returns(linkedStatement1.Object);
            statement.SetupGet(x => x.Name).Returns("David");
            statement.SetupGet(x => x.ScopeType).Returns(typeof(int));
            statement.SetupGet(x => x.TransformsScopeTo).Returns((Type)null);

            var statementSet = new StatementSet().Add(statement.Object);

            var ast = new DaisyAst(new GroupOperatorNode("Hello gov'nor",new StatementNode("Blah")));

            var load = new DaisyLinker(ast,statementSet,typeof(int));
            var ex = Assert.Catch<FailedLinkException>(() => load.Link());
            Assert.AreEqual(1, ex.Errors.Count);
            Assert.IsInstanceOf<NoLinksPermittedError>(ex.Errors.First());
        }
    }
}
