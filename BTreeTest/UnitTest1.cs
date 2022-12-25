using BTreeNewVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace BTreeTest;

public class UnitTest1
{
    [Fact]
    public void DataLossTest()
    {
        var btree = new BTree<int>(3);
        var expectedCurrent = new List<int>();
        var actualCurrent = new List<int>();
        var numCount = 10000;
        for (int i = 0; i < numCount; i++)
        {
            btree.AddNewValue(i);
            expectedCurrent.Add(i);
        }
        expectedCurrent.Sort();

        actualCurrent.AddRange(GetValueAfterHead(btree._head));

        actualCurrent.AddRange(GetValueNode(btree._head.HeadCell));
        actualCurrent.Sort();

        Assert.Equal(expectedCurrent, actualCurrent);
    }
    private IEnumerable<int> GetValueAfterHead(Node<int> node)
    {
        var cell = node.HeadCell;

        while (cell != null)
        {
            if (cell.LeftNode != null)
            {
                foreach (var item in GetValue(cell.LeftNode))
                {
                    yield return item;
                }
            }

            if (cell.RightNode != null && cell.NextCell == null) 
            {
                foreach (var item in GetValue(cell.RightNode))
                {
                    yield return item;
                }
            }

            cell = cell.NextCell;
        }
    }

    private List<int> GetValue(Node<int> node)
    {
        var cell = node.HeadCell;
        var values = new List<int>();
        values.AddRange(GetValueNode(cell));

        while (cell != null)
        {
            if (cell.LeftNode is null)
            {
                break;
            }

            values.AddRange(GetValue(cell.LeftNode));
            if (cell.NextCell is null)
            {
                values.AddRange(GetValue(cell.RightNode));
            }
            cell = cell.NextCell;
        }

        return values;
    }

    private List<int> GetValueNode(Cell<int> cell)
    {
        var list = new List<int>();
        var count = cell.CountCell();
        for (int i = 0; i < count; i++)
        {
            list.Add(cell.Value);
            cell = cell.NextCell;
        }
        return list;
    }

    [Fact]
    public void BalanceTest()
    {
        // arrange

        var random = new Random((int)DateTime.Now.Ticks);

        var btree = new BTree<int>(3);

        // act

        for (var i = 0; i < 10000; i++)
        {
            btree.AddNewValue(random.Next(int.MinValue, int.MaxValue));

            // assert

            var deeps = GetDeepAfterHead(btree._head).ToList();
            if (deeps.Any())
            {
                var firstCount = deeps.First();
                Assert.True(deeps.All(x => x == firstCount));
            }
        }
    }

    private IEnumerable<int> GetDeepAfterHead(Node<int> node)
    {
        var cell = node.HeadCell;

        while (cell != null)
        {
            if (cell.LeftNode is null)
            {
                break;
            }

            yield return GetDeep(cell.LeftNode);
            yield return GetDeep(cell.RightNode);

            cell = cell.NextCell;
        }
    }

    private int GetDeep(Node<int> node)
    {
        var result = 1;
        var cell = node.HeadCell;
        var deeps = new List<int>();

        while (cell != null)
        {
            if (cell.LeftNode is null)
            {
                break;
            }

            deeps.Add(GetDeep(cell.LeftNode));
            deeps.Add(GetDeep(cell.RightNode));

            cell = cell.NextCell;
        }

        if (deeps.Count > 0)
        {
            var max = int.MinValue;

            for (var i = 0; i < deeps.Count; i++)
            {
                if (deeps[0] > max)
                {
                    max = deeps[0];
                }
            }

            result += max;
        }

        return result;

    }
    [Fact]
    public void ParanteTest()
    {
        // arrange

        var random = new Random(5);

        var btree = new BTree<int>(9);

        // act

        for (var i = 0; i < 100000; i++)
        {
            btree.AddNewValue(random.Next(int.MinValue, int.MaxValue));

            // assert

            AssertParant(btree._head);
        }
    }

    private void AssertParant(Node<int> node)
    {
        var cell = node.HeadCell;

        while (cell != null)
        {
            if (cell.LeftNode is null)
            {
                break;
            }

            Assert.Equal(node, cell.LeftNode.ParentNode);
            AssertParant(cell.LeftNode);
            Assert.Equal(node, cell.RightNode!.ParentNode);
            AssertParant(cell.RightNode);

            cell = cell.NextCell;
        }
    }
    [Fact]
    public void AddLastTest()
    {
        // arrange
        int valueFirstCell = 35;
        int valueSecondCell = 45;
        int valueFhirdCell = 55;
        Cell<int> firstCell = new Cell<int>(valueFirstCell);
        Cell<int> secondCell = new Cell<int>(valueSecondCell);
        Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
        Node<int> node = new Node<int>(4, firstCell);
        var expectedCurrent = valueFhirdCell;
        // act
        node.HeadCell.AddLastCell(secondCell);
        node.HeadCell.AddLastCell(fhirdCell);
        var actualCurrent = node.HeadCell.NextCell.NextCell.Value;
        // assert
        Assert.Equal(expectedCurrent, actualCurrent);
    }

    [Fact]

    public void DeleteCellByIndexTest()
    {
        // arrange
        int valueFirstCell = 35;
        int valueSecondCell = 45;
        int valueFhirdCell = 55;
        int valueFourthCell = 65;
        int valueFifthCell = 75;
        Cell<int> firstCell = new Cell<int>(valueFirstCell);
        Cell<int> secondCell = new Cell<int>(valueSecondCell);
        Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
        Cell<int> fourthCell = new Cell<int>(valueFourthCell);
        Cell<int> fifthCell = new Cell<int>(valueFifthCell);
        Node<int> node = new Node<int>(3, firstCell);
        node.HeadCell.AddLastCell(secondCell);
        node.HeadCell.AddLastCell(fhirdCell);
        node.HeadCell.AddLastCell(fourthCell);
        node.HeadCell.AddLastCell(fifthCell);
        var expectedCurrent = valueFifthCell;
        // act
        node.HeadCell.DeleteCellByIndex(1);
        node.HeadCell.DeleteCellByIndex(2);
        var actualCurrent = node.HeadCell.NextCell.NextCell.Value;
        // assert
        Assert.Equal(expectedCurrent, actualCurrent);
    }

    [Fact]
    public void DeleteAllCellAfterIndexTest()
    {
        // arrange
        int valueFirstCell = 35;
        int valueSecondCell = 45;
        int valueFhirdCell = 55;
        int valueFourthCell = 65;
        int valueFifthCell = 75;
        Cell<int> firstCell = new Cell<int>(valueFirstCell);
        Cell<int> secondCell = new Cell<int>(valueSecondCell);
        Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
        Cell<int> fourthCell = new Cell<int>(valueFourthCell);
        Cell<int> fifthCell = new Cell<int>(valueFifthCell);
        Node<int> node = new Node<int>(3, firstCell);
        node.HeadCell.AddLastCell(secondCell);
        node.HeadCell.AddLastCell(fhirdCell);
        node.HeadCell.AddLastCell(fourthCell);
        node.HeadCell.AddLastCell(fifthCell);

        // act
        node.HeadCell.DeleteAllCellAfterIndex(2);
        var expectedCurrent = node.HeadCell.NextCell.NextCell.NextCell;
        //assert
        Assert.Null(expectedCurrent);
        Assert.NotNull(node.HeadCell.NextCell.NextCell);
    }

    [Fact]

    public void CountNodeTest()
    {
        // arrange
        int valueFirstCell = 35;
        int valueSecondCell = 45;
        int valueFhirdCell = 55;
        Cell<int> firstCell = new Cell<int>(valueFirstCell);
        Cell<int> secondCell = new Cell<int>(valueSecondCell);
        Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
        Node<int> node = new Node<int>(3, firstCell);
        node.HeadCell.AddLastCell(secondCell);
        node.HeadCell.AddLastCell(fhirdCell);
        var expectedContent = 3;

        // act 
        var actualContent = node.HeadCell.CountCell();
        // assert
        Assert.Equal(expectedContent, actualContent);

    }

    [Fact]

    public void GetCellByIndexTest()
    {
        // arrange

        int valueFirstCell = 35;
        int valueSecondCell = 45;
        int valueFhirdCell = 55;
        Cell<int> firstCell = new Cell<int>(valueFirstCell);
        Cell<int> secondCell = new Cell<int>(valueSecondCell);
        Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
        Node<int> node = new Node<int>(3, firstCell);
        node.HeadCell.AddLastCell(secondCell);
        node.HeadCell.AddLastCell(fhirdCell);

        var expectedContent1 = firstCell;
        var expectedContent2 = secondCell;
        var expectedContent3 = fhirdCell;

        // act
        var actualContent1 = node.HeadCell.GetCellByIndex(0);
        var actualContent2 = node.HeadCell.GetCellByIndex(1);
        var actualContent3 = node.HeadCell.GetCellByIndex(2);

        // assert
        Assert.Equal(expectedContent1, actualContent1);
        Assert.Equal(expectedContent2, actualContent2);
        Assert.Equal(expectedContent3, actualContent3);
    }

    [Fact]

    public void AddNewCellInNotFullNodeTest()
    {
        // arrange
        int valueFirstCell = 35;
        int valueSecondCell = 45;
        int valueFhirdCell = 55;
        int valueFourthCell = 65;
        int valueFifthCell = 75;
        Cell<int> firstCell = new Cell<int>(valueFirstCell);
        Cell<int> secondCell = new Cell<int>(valueSecondCell);
        Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
        Cell<int> fourthCell = new Cell<int>(valueFourthCell);
        Cell<int> fifthCell = new Cell<int>(valueFifthCell);
        Node<int> node = new Node<int>(6, secondCell);
        var expectedContent1 = firstCell;
        var expectedContent2 = secondCell;
        var expectedContent3 = fhirdCell;
        var expectedContent4 = fourthCell;
        var expectedContent5 = fifthCell;


        // act
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(firstCell.Value));
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(fhirdCell.Value));
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(fifthCell.Value));
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(fourthCell.Value));
        var actualContent1 = node.HeadCell.GetCellByIndex(0);
        var actualContent2 = node.HeadCell.GetCellByIndex(1);
        var actualContent3 = node.HeadCell.GetCellByIndex(2);
        var actualContent4 = node.HeadCell.GetCellByIndex(3);
        var actualContent5 = node.HeadCell.GetCellByIndex(4);

        // assert
        Assert.Equal(expectedContent1.Value, actualContent1.Value);
        Assert.Equal(expectedContent2.Value, actualContent2.Value);
        Assert.Equal(expectedContent3.Value, actualContent3.Value);
        Assert.Equal(expectedContent4.Value, actualContent4.Value);
        Assert.Equal(expectedContent5.Value, actualContent5.Value);

    }

    [Fact]

    public void SearchMedianValueTestEven1()
    {
        //  arrange
        var dto = new DataTransferObject<int>(456);
        int valueFirstCell = 35;
        int valueSecondCell = 45;
        int valueFhirdCell = 55;
        int valueFourthCell = 65;
        Cell<int> firstCell = new Cell<int>(valueFirstCell);
        Cell<int> secondCell = new Cell<int>(valueSecondCell);
        Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
        Cell<int> fourthCell = new Cell<int>(valueFourthCell);
        Node<int> node = new Node<int>(5, secondCell);
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(firstCell.Value));
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(fhirdCell.Value));
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(fourthCell.Value));
        var expectedContent = 2;
        // act
        var actualContent = node.SearchMedianValue(dto.Value);
        // assert
        Assert.Equal(expectedContent, actualContent);
    }

    [Fact]

    public void SearchMedianValueTestEven2()
    {
        //  arrange
        var dto = new DataTransferObject<int>(21);
        int valueFirstCell = 35;
        int valueSecondCell = 45;
        int valueFhirdCell = 55;
        int valueFourthCell = 65;
        Cell<int> firstCell = new Cell<int>(valueFirstCell);
        Cell<int> secondCell = new Cell<int>(valueSecondCell);
        Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
        Cell<int> fourthCell = new Cell<int>(valueFourthCell);
        Node<int> node = new Node<int>(5, secondCell);
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(firstCell.Value));
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(fhirdCell.Value));
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(fourthCell.Value));
        var expectedContent = 1;
        // act
        var actualContent = node.SearchMedianValue(dto.Value);
        // assert
        Assert.Equal(expectedContent, actualContent);
    }

    [Fact]
    public void SearchMedianValueTestEven3()
    {
        //  arrange
        var dto = new DataTransferObject<int>(52);
        int valueFirstCell = 35;
        int valueSecondCell = 45;
        int valueFhirdCell = 55;
        int valueFourthCell = 65;
        Cell<int> firstCell = new Cell<int>(valueFirstCell);
        Cell<int> secondCell = new Cell<int>(valueSecondCell);
        Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
        Cell<int> fourthCell = new Cell<int>(valueFourthCell);
        Node<int> node = new Node<int>(5, secondCell);
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(firstCell.Value));
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(fhirdCell.Value));
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(fourthCell.Value));
        var expectedContent = -1;
        // act
        var actualContent = node.SearchMedianValue(dto.Value);
        // assert
        Assert.Equal(expectedContent, actualContent);
    }

    [Fact]
    public void SearchMedianValueTestOdd1()
    {
        //  arrange
        var dto = new DataTransferObject<int>(446);
        int valueFirstCell = 35;
        int valueSecondCell = 45;
        int valueFhirdCell = 55;
        Cell<int> firstCell = new Cell<int>(valueFirstCell);
        Cell<int> secondCell = new Cell<int>(valueSecondCell);
        Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
        Node<int> node = new Node<int>(4, secondCell);
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(firstCell.Value));
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(fhirdCell.Value));
        var expectedContent = 1;
        // act
        var actualContent = node.SearchMedianValue(dto.Value);
        // assert
        Assert.Equal(expectedContent, actualContent);

    }

    [Fact]
    public void SearchMedianValueTestOdd2()
    {
        //  arrange
        var dto = new DataTransferObject<int>(46);
        int valueFirstCell = 35;
        int valueSecondCell = 45;
        int valueFhirdCell = 55;
        Cell<int> firstCell = new Cell<int>(valueFirstCell);
        Cell<int> secondCell = new Cell<int>(valueSecondCell);
        Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
        Node<int> node = new Node<int>(3, secondCell);
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(firstCell.Value));
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(fhirdCell.Value));
        var expectedContent = -1;

        // act
        var actualContent = node.SearchMedianValue(dto.Value);
        // assert
        Assert.Equal(expectedContent, actualContent);
    }

    [Fact]
    public void SearchMedianValueTestOdd3()
    {
        //  arrange
        var dto = new DataTransferObject<int>(22);
        int valueFirstCell = 35;
        int valueSecondCell = 45;
        int valueFhirdCell = 55;
        Cell<int> firstCell = new Cell<int>(valueFirstCell);
        Cell<int> secondCell = new Cell<int>(valueSecondCell);
        Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
        Node<int> node = new Node<int>(3, secondCell);
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(firstCell.Value));
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(fhirdCell.Value));
        var expectedContent = 1;
        // act
        var actualContent = node.SearchMedianValue(dto.Value);
        // assert
        Assert.Equal(expectedContent, actualContent);
    }

    [Fact]

    public void DivisionNodeTestOdd()
    {
        // actual
        int valueFirstCell = 35;
        int valueSecondCell = 45;
        int newValue = 56;
        int index = 1;
        var dto = new DataTransferObject<int>(newValue);
        Cell<int> firstCell = new Cell<int>(valueFirstCell);
        Cell<int> secondCell = new Cell<int>(valueSecondCell);
        Node<int> node = new Node<int>(3, firstCell);
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(secondCell.Value));
        var expectedCurrentHead = valueSecondCell;
        var expectedCurrentLeftNode = valueFirstCell;
        var expectedCurrentRightNode = newValue;
        
        //act
        var resultCell = node.DivisionNode(index, dto);
        var actualCurrentHead = resultCell.Value;
        var actualCurrentLefttNode = resultCell.LeftNode.HeadCell.Value;
        var actualCurrentRightNode = resultCell.RightNode.HeadCell.Value;

        //assert
        Assert.NotNull(resultCell.LeftNode);
        Assert.NotNull(resultCell.RightNode);
        Assert.Equal(expectedCurrentHead, actualCurrentHead);
        Assert.Equal(expectedCurrentLeftNode, actualCurrentLefttNode);
        Assert.Equal(expectedCurrentRightNode, actualCurrentRightNode);
        Assert.Null(resultCell.LeftNode.HeadCell.NextCell);
        Assert.Null(resultCell.RightNode.HeadCell.NextCell);
    }

    [Fact]

    public void DivisionNodeTestEven()
    {
        // actual
        int valueFirstCell = 35;
        int valueSecondCell = 45;
        int valueFhirdCell = 55;
        int newValue = 44;
        int index = -1;
        var dto = new DataTransferObject<int>(newValue);
        Cell<int> firstCell = new Cell<int>(valueFirstCell);
        Cell<int> secondCell = new Cell<int>(valueSecondCell);
        Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
        Node<int> node = new Node<int>(4, firstCell);
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(secondCell.Value));
        node.AddNewCellInNotFullNode(new DataTransferObject<int>(fhirdCell.Value));
        var expectedCurrentHead = newValue;
        var expectedCurrentLeftNode = valueFirstCell;
        var expectedCurrentRightNode = valueSecondCell;

        //act
        var resultCell = node.DivisionNode(index, dto);
        var actualCurrentHead = resultCell.Value;
        var actualCurrentLefttNode = resultCell.LeftNode.HeadCell.Value;
        var actualCurrentRightNode = resultCell.RightNode.HeadCell.Value;

        //assert
        Assert.NotNull(resultCell.LeftNode);
        Assert.NotNull(resultCell.RightNode);
        Assert.Equal(expectedCurrentHead, actualCurrentHead);
        Assert.Equal(expectedCurrentLeftNode, actualCurrentLefttNode);
        Assert.Equal(expectedCurrentRightNode, actualCurrentRightNode);
        Assert.Null(resultCell.LeftNode.HeadCell.NextCell);
        Assert.NotNull(resultCell.RightNode.HeadCell.NextCell);
    }
    //[Fact]
    //public void DeleteCellByValueTest()
    //{
    //    // actual
    //    int valueFirstCell = 35;
    //    int valueSecondCell = 45;
    //    int valueFhirdCell = 55;
    //    int valueFourthCell = 65;
    //    int valueFifthCell = 75;
    //    Cell<int> firstCell = new Cell<int>(valueFirstCell);
    //    Cell<int> secondCell = new Cell<int>(valueSecondCell);
    //    Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
    //    Cell<int> fourthCell = new Cell<int>(valueFourthCell);
    //    Cell<int> fifthCell = new Cell<int>(valueFifthCell);
    //    Node<int> node = new Node<int>(6, firstCell);
    //    node.AddNewCellInNotFullNode(secondCell);
    //    node.AddNewCellInNotFullNode(fhirdCell);
    //    node.AddNewCellInNotFullNode(fourthCell);
    //    node.AddNewCellInNotFullNode(fifthCell);
    //    // act
    //    node.FirstCellInNode.DeleteCellByValue(valueFhirdCell);
    //    // assert
    //    Assert.Null(node.FirstCellInNode.NextCell.NextCell);
    //}

    //[Fact]
    //public void DeleteCellByValueTest2()
    //{
    //    // actual
    //    int valueFirstCell = 35;
    //    int valueSecondCell = 45;
    //    int valueFhirdCell = 55;
    //    int valueFourthCell = 65;
    //    int valueFifthCell = 75;
    //    Cell<int> firstCell = new Cell<int>(valueFirstCell);
    //    Cell<int> secondCell = new Cell<int>(valueSecondCell);
    //    Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
    //    Cell<int> fourthCell = new Cell<int>(valueFourthCell);
    //    Cell<int> fifthCell = new Cell<int>(valueFifthCell);
    //    Node<int> node = new Node<int>(6, firstCell);
    //    node.AddNewCellInNotFullNode(secondCell);
    //    node.AddNewCellInNotFullNode(fhirdCell);
    //    node.AddNewCellInNotFullNode(fourthCell);
    //    node.AddNewCellInNotFullNode(fifthCell);
    //    // act
    //    node.FirstCellInNode.DeleteCellByValue(valueFourthCell);
    //    // assert
    //    Assert.Null(node.FirstCellInNode.NextCell.NextCell.NextCell);

    //}
}
