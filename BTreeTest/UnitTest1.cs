using BTreeNewVersion;
using Xunit;


namespace BTreeTest;

public class UnitTest1
{
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
        Node<int> node = new Node<int>(3, firstCell);
        Cell<int> firstCell2 = new Cell<int>(valueFirstCell);
        Cell<int> secondCell2 = new Cell<int>(valueSecondCell);
        Cell<int> fhirdCell2 = new Cell<int>(valueFhirdCell);
        Node<int> node2 = new Node<int>(3, firstCell2);
        // act
        node.FirstCellInNode.AddLastCell(secondCell);
        node.FirstCellInNode.AddLastCell(fhirdCell);
        node2.FirstCellInNode.AddLastCell(secondCell2);
        node2.FirstCellInNode.AddLastCell(fhirdCell2);
        node.FirstCellInNode.AddLastCell(node2.FirstCellInNode);

        // assert
        Assert.Null(node.FirstCellInNode.NextCell.NextCell.NextCell.NextCell);
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
        node.FirstCellInNode.AddLastCell(secondCell);
        node.FirstCellInNode.AddLastCell(fhirdCell);
        node.FirstCellInNode.AddLastCell(fourthCell);
        node.FirstCellInNode.AddLastCell(fifthCell);
        var expectedCurrent = valueFifthCell;
        // act
        node.FirstCellInNode.DeleteCellByIndex(1);
        node.FirstCellInNode.DeleteCellByIndex(2);
        var actualCurrent = node.FirstCellInNode.NextCell.NextCell.Value;
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
        node.FirstCellInNode.AddLastCell(secondCell);
        node.FirstCellInNode.AddLastCell(fhirdCell);
        node.FirstCellInNode.AddLastCell(fourthCell);
        node.FirstCellInNode.AddLastCell(fifthCell);

        // act
        node.FirstCellInNode.DeleteAllCellAfterIndex(2);
        var expectedCurrent = node.FirstCellInNode.NextCell.NextCell.NextCell;
        //assert
        Assert.Null(expectedCurrent);
        Assert.NotNull(node.FirstCellInNode.NextCell.NextCell);
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
        node.FirstCellInNode.AddLastCell(secondCell);
        node.FirstCellInNode.AddLastCell(fhirdCell);
        var expectedContent = 3;

        // act 
        var actualContent = node.FirstCellInNode.CountCell();
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
        node.FirstCellInNode.AddLastCell(secondCell);
        node.FirstCellInNode.AddLastCell(fhirdCell);

        var expectedContent1 = firstCell;
        var expectedContent2 = secondCell;
        var expectedContent3 = fhirdCell;

        // act
        var actualContent1 = node.FirstCellInNode.GetCellByIndex(0);
        var actualContent2 = node.FirstCellInNode.GetCellByIndex(1);
        var actualContent3 = node.FirstCellInNode.GetCellByIndex(2);

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
        var actualContent1 = node.FirstCellInNode.GetCellByIndex(0);
        var actualContent2 = node.FirstCellInNode.GetCellByIndex(1);
        var actualContent3 = node.FirstCellInNode.GetCellByIndex(2);
        var actualContent4 = node.FirstCellInNode.GetCellByIndex(3);
        var actualContent5 = node.FirstCellInNode.GetCellByIndex(4);

        // assert
        Assert.Equal(expectedContent1.Value, actualContent1.Value);
        Assert.Equal(expectedContent2.Value, actualContent2.Value);
        Assert.Equal(expectedContent3.Value, actualContent3.Value);
        Assert.Equal(expectedContent4.Value, actualContent4.Value);
        Assert.Equal(expectedContent5.Value, actualContent5.Value);

    }

    //[Fact]

    //public void SearchMedianValueTestEven1()
    //{
    //    //  arrange
    //    var dto = new DataTransferObject<int>(456);
    //    int valueFirstCell = 35;
    //    int valueSecondCell = 45;
    //    int valueFhirdCell = 55;
    //    int valueFourthCell = 65;
    //    Cell<int> firstCell = new Cell<int>(valueFirstCell);
    //    Cell<int> secondCell = new Cell<int>(valueSecondCell);
    //    Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
    //    Cell<int> fourthCell = new Cell<int>(valueFourthCell);
    //    Node<int> node = new Node<int>(5, secondCell);
    //    node.AddNewCellInNotFullNode(new DataTransferObject<int>(firstCell.Value));
    //    node.AddNewCellInNotFullNode(new DataTransferObject<int>(fhirdCell.Value));
    //    node.AddNewCellInNotFullNode(new DataTransferObject<int>(fourthCell.Value));
    //    var expectedContent = fhirdCell.Value;
    //    var expectedContent1 = valueFirstCell;
    //    var expectedContent2 = valueSecondCell;
    //    var expectedContent3 = valueFourthCell;
    //    var expectedContent4 = dto.Value;
    //    // act
    //    var actualContent = node.SearchMedianValue(dto).Value;
    //    var actualContent1 = node.FirstCellInNode.Value;
    //    var actualContent2 = node.FirstCellInNode.NextCell.Value;
    //    var actualContent3 = node.FirstCellInNode.NextCell.NextCell.Value;
    //    var actualContent4 = node.FirstCellInNode.NextCell.NextCell.NextCell.Value;
    //    // assert
    //    Assert.Equal(expectedContent, actualContent);
    //    Assert.Equal(expectedContent1, actualContent1);
    //    Assert.Equal(expectedContent2, actualContent2);
    //    Assert.Equal(expectedContent3, actualContent3);
    //    Assert.Equal(expectedContent4, actualContent4);
    //}

    //[Fact]

    //public void SearchMedianValueTestEven2()
    //{
    //    //  arrange
    //    var dto = new DataTransferObject<int>(21);
    //    int valueFirstCell = 35;
    //    int valueSecondCell = 45;
    //    int valueFhirdCell = 55;
    //    int valueFourthCell = 65;
    //    Cell<int> firstCell = new Cell<int>(valueFirstCell);
    //    Cell<int> secondCell = new Cell<int>(valueSecondCell);
    //    Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
    //    Cell<int> fourthCell = new Cell<int>(valueFourthCell);
    //    Node<int> node = new Node<int>(5, secondCell);
    //    node.AddNewCellInNotFullNode(new DataTransferObject<int>(firstCell.Value));
    //    node.AddNewCellInNotFullNode(new DataTransferObject<int>(fhirdCell.Value));
    //    node.AddNewCellInNotFullNode(new DataTransferObject<int>(fourthCell.Value));
    //    var expectedContent = valueSecondCell;
    //    var expectedContent1 = dto.Value;
    //    var expectedContent2 = valueFirstCell;
    //    var expectedContent3 = valueFhirdCell;
    //    var expectedContent4 = valueFourthCell;
    //    // act
    //    var actualContent = node.SearchMedianValue(dto).Value;
    //    var actualContent1 = node.FirstCellInNode.Value;
    //    var actualContent2 = node.FirstCellInNode.NextCell.Value;
    //    var actualContent3 = node.FirstCellInNode.NextCell.NextCell.Value;
    //    var actualContent4 = node.FirstCellInNode.NextCell.NextCell.NextCell.Value;
    //    // assert
    //    Assert.Equal(expectedContent, actualContent);
    //    Assert.Equal(expectedContent1, actualContent1);
    //    Assert.Equal(expectedContent2, actualContent2);
    //    Assert.Equal(expectedContent3, actualContent3);
    //    Assert.Equal(expectedContent4, actualContent4);
    //}

    //[Fact]
    //public void SearchMedianValueTestEven3()
    //{
    //    //  arrange
    //    var dto = new DataTransferObject<int>(52);
    //    int valueFirstCell = 35;
    //    int valueSecondCell = 45;
    //    int valueFhirdCell = 55;
    //    int valueFourthCell = 65;
    //    Cell<int> firstCell = new Cell<int>(valueFirstCell);
    //    Cell<int> secondCell = new Cell<int>(valueSecondCell);
    //    Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
    //    Cell<int> fourthCell = new Cell<int>(valueFourthCell);
    //    Node<int> node = new Node<int>(5, secondCell);
    //    node.AddNewCellInNotFullNode(new DataTransferObject<int>(firstCell.Value));
    //    node.AddNewCellInNotFullNode(new DataTransferObject<int>(fhirdCell.Value));
    //    node.AddNewCellInNotFullNode(new DataTransferObject<int>(fourthCell.Value));
    //    var expectedContent = dto.Value;
    //    var expectedContent1 = valueFirstCell;
    //    var expectedContent2 = valueSecondCell;
    //    var expectedContent3 = valueFhirdCell;
    //    var expectedContent4 = valueFourthCell;
    //    // act
    //    var actualContent = node.SearchMedianValue(dto).Value;
    //    var actualContent1 = node.FirstCellInNode.Value;
    //    var actualContent2 = node.FirstCellInNode.NextCell.Value;
    //    var actualContent3 = node.FirstCellInNode.NextCell.NextCell.Value;
    //    var actualContent4 = node.FirstCellInNode.NextCell.NextCell.NextCell.Value;
    //    // assert
    //    Assert.Equal(expectedContent, actualContent);
    //    Assert.Equal(expectedContent1, actualContent1);
    //    Assert.Equal(expectedContent2, actualContent2);
    //    Assert.Equal(expectedContent3, actualContent3);
    //    Assert.Equal(expectedContent4, actualContent4);
    //}

    //[Fact]
    //public void SearchMedianValueTestOdd1()
    //{
    //    //  arrange
    //    var dto = new DataTransferObject<int>(446);
    //    int valueFirstCell = 35;
    //    int valueSecondCell = 45;
    //    int valueFhirdCell = 55;
    //    Cell<int> firstCell = new Cell<int>(valueFirstCell);
    //    Cell<int> secondCell = new Cell<int>(valueSecondCell);
    //    Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
    //    Node<int> node = new Node<int>(3, secondCell);
    //    node.AddNewCellInNotFullNode(new DataTransferObject<int>(firstCell.Value));
    //    node.AddNewCellInNotFullNode(new DataTransferObject<int>(fhirdCell.Value));
    //    var expectedContent = valueSecondCell;
    //    var expectedContent1 = valueFirstCell;
    //    var expectedContent2 = valueFhirdCell;
    //    var expectedContent4 = dto.Value;
    //    // act
    //    var actualContent = node.SearchMedianValue(dto).Value;
    //    var actualContent1 = node.FirstCellInNode.Value;
    //    var actualContent2 = node.FirstCellInNode.NextCell.Value;
    //    var actualContent4 = node.FirstCellInNode.NextCell.NextCell.Value;
    //    // assert
    //    Assert.Equal(expectedContent, actualContent);
    //    Assert.Equal(expectedContent1, actualContent1);
    //    Assert.Equal(expectedContent2, actualContent2);
    //    Assert.Equal(expectedContent4, actualContent4);
    //}

    //[Fact]
    //public void SearchMedianValueTestOdd2()
    //{
    //    //  arrange
    //    var dto = new DataTransferObject<int>(46);
    //    int valueFirstCell = 35;
    //    int valueSecondCell = 45;
    //    int valueFhirdCell = 55;
    //    Cell<int> firstCell = new Cell<int>(valueFirstCell);
    //    Cell<int> secondCell = new Cell<int>(valueSecondCell);
    //    Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
    //    Node<int> node = new Node<int>(3, secondCell);
    //    node.AddNewCellInNotFullNode(new DataTransferObject<int>(firstCell.Value));
    //    node.AddNewCellInNotFullNode(new DataTransferObject<int>(fhirdCell.Value));
    //    var expectedContent = dto.Value;
    //    var expectedContent1 = valueFirstCell;
    //    var expectedContent2 = valueSecondCell;
    //    var expectedContent4 = valueFhirdCell;
    //    // act
    //    var actualContent = node.SearchMedianValue(dto).Value;
    //    var actualContent1 = node.FirstCellInNode.Value;
    //    var actualContent2 = node.FirstCellInNode.NextCell.Value;
    //    var actualContent4 = node.FirstCellInNode.NextCell.NextCell.Value;
    //    // assert
    //    Assert.Equal(expectedContent, actualContent);
    //    Assert.Equal(expectedContent1, actualContent1);
    //    Assert.Equal(expectedContent2, actualContent2);
    //    Assert.Equal(expectedContent4, actualContent4);
    //}

    //[Fact]
    //public void SearchMedianValueTestOdd3()
    //{
    //    //  arrange
    //    var dto = new DataTransferObject<int>(22);
    //    int valueFirstCell = 35;
    //    int valueSecondCell = 45;
    //    int valueFhirdCell = 55;
    //    Cell<int> firstCell = new Cell<int>(valueFirstCell);
    //    Cell<int> secondCell = new Cell<int>(valueSecondCell);
    //    Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
    //    Node<int> node = new Node<int>(3, secondCell);
    //    node.AddNewCellInNotFullNode(new DataTransferObject<int>(firstCell.Value));
    //    node.AddNewCellInNotFullNode(new DataTransferObject<int>(fhirdCell.Value));
    //    var expectedContent = valueSecondCell;
    //    var expectedContent1 = dto.Value;
    //    var expectedContent2 = valueFirstCell;
    //    var expectedContent4 = valueFhirdCell;
    //    // act
    //    var actualContent = node.SearchMedianValue(dto).Value;
    //    var actualContent1 = node.FirstCellInNode.Value;
    //    var actualContent2 = node.FirstCellInNode.NextCell.Value;
    //    var actualContent4 = node.FirstCellInNode.NextCell.NextCell.Value;
    //    // assert
    //    Assert.Equal(expectedContent, actualContent);
    //    Assert.Equal(expectedContent1, actualContent1);
    //    Assert.Equal(expectedContent2, actualContent2);
    //    Assert.Equal(expectedContent4, actualContent4);
    //}

    //[Fact]

    //public void DivisionNodeTest()
    //{
    //    // actual
    //    int valueFirstCell = 35;
    //    int valueSecondCell = 45;
    //    int valueFhirdCell = 55;
    //    int valueFourthCell = 65;
    //    int valueFifthCell = 75;
    //    int newValue = 55;
    //    var dto = new DataTransferObject<int>(newValue);
    //    Cell<int> firstCell = new Cell<int>(valueFirstCell);
    //    Cell<int> secondCell = new Cell<int>(valueSecondCell);
    //    Cell<int> fhirdCell = new Cell<int>(valueFhirdCell);
    //    Cell<int> fourthCell = new Cell<int>(valueFourthCell);
    //    Cell<int> fifthCell = new Cell<int>(valueFifthCell);
    //    Node<int> node = new Node<int>(6, firstCell);
    //    node.AddNewCellInNotFullNode(new DataTransferObject<int>(secondCell.Value));
    //    node.AddNewCellInNotFullNode(new DataTransferObject<int>(fhirdCell.Value));
    //    node.AddNewCellInNotFullNode(new DataTransferObject<int>(fourthCell.Value));
    //    node.AddNewCellInNotFullNode(new DataTransferObject<int>(fifthCell.Value));
    //    var expectedCurrent1 = valueFirstCell;
    //    var expectedCurrent2 = valueSecondCell;
    //    var expectedCurrent3 = valueFhirdCell;
    //    var expectedCurrent4 = valueFourthCell;
    //    var expectedCurrent5 = valueFifthCell;

    //    //act
    //    var resultCell = node.DivisionNode(dto);
    //    var actualCurrentLefttNode1 = resultCell.LeftNode.FirstCellInNode.Value;
    //    var actualCurrentLefttNode2 = resultCell.LeftNode.FirstCellInNode.NextCell.Value;
    //    var actualCurrentRightNode1 = resultCell.RightNode.FirstCellInNode.Value;
    //    var actualCurrentRightNode2 = resultCell.RightNode.FirstCellInNode.NextCell.Value;
    //    var actualCurrentRightNode3 = resultCell.RightNode.FirstCellInNode.NextCell.NextCell.Value;

    //    //assert
    //    Assert.NotNull(resultCell.LeftNode);
    //    Assert.NotNull(resultCell.RightNode);
    //    Assert.Equal(expectedCurrent1, actualCurrentLefttNode1);
    //    Assert.Equal(expectedCurrent2, actualCurrentLefttNode2);
    //    Assert.Equal(expectedCurrent3, actualCurrentRightNode1);
    //    Assert.Equal(expectedCurrent4, actualCurrentRightNode2);
    //    Assert.Equal(expectedCurrent5, actualCurrentRightNode3);
    //    Assert.Null(resultCell.LeftNode.FirstCellInNode.NextCell.NextCell);
    //    Assert.Null(resultCell.RightNode.FirstCellInNode.NextCell.NextCell.NextCell);
    //}

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
