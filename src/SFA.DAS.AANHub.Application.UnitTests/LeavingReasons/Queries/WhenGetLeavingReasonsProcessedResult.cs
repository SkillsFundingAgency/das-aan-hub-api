namespace SFA.DAS.AANHub.Application.UnitTests.LeavingReasons.Queries;


//MFCMFC
//public class WhenGetLeavingReasonsProcessedResult
//{
//    public readonly List<LeavingReason> LeavingReasons = new();

//    private readonly LeavingReason _leavingReasonCategory2Reason3 = new() { Category = "category 2", Description = "leaving reason 3", Id = 1, Ordering = 3 };
//    private readonly LeavingReason _leavingReasonCategory2Reason1 = new() { Category = "category 2", Description = "leaving reason 1", Id = 2, Ordering = 1 };
//    private readonly LeavingReason _leavingReasonCategory2Reason2 = new() { Category = "category 2", Description = "leaving reason 2", Id = 3, Ordering = 2 };

//    private readonly LeavingReason _leavingReasonCategory1Reason1 = new() { Category = "category 1", Description = "leaving reason 1", Id = 4, Ordering = 1 };

//    private readonly LeavingReason _leavingReasonCategory3Reason1 = new() { Category = "category 3", Description = "leaving reason 1", Id = 5, Ordering = 1 };


//    [SetUp]
//    public void Init()
//    {
//        LeavingReasons.Clear();

//        LeavingReasons.Add(_leavingReasonCategory1Reason1);
//        LeavingReasons.Add(_leavingReasonCategory3Reason1);
//        LeavingReasons.Add(_leavingReasonCategory2Reason3);
//        LeavingReasons.Add(_leavingReasonCategory2Reason1);
//        LeavingReasons.Add(_leavingReasonCategory2Reason2);
//    }

//    [Test]
//    public void WhenLeavingReasons_ProcessedResult_CheckNumberOfCategories()
//    {
//        var result = (GetLeavingReasonsQueryResult)LeavingReasons;
//        result.ProcessedResult.LeavingCategories.Count.Should().Be(3);
//    }

//    [Test]
//    public void WhenLeavingReasons_ProcessedResult_CheckFirstCategory()
//    {
//        var result = (GetLeavingReasonsQueryResult)LeavingReasons;

//        var category1 = result.ProcessedResult.LeavingCategories.First();

//        category1.LeavingReasons.Count.Should().Be(1);

//        category1.LeavingReasons.First().Description.Should().BeEquivalentTo(_leavingReasonCategory1Reason1.Description);
//        category1.LeavingReasons.First().Id.Should().Be(_leavingReasonCategory1Reason1.Id);
//        category1.LeavingReasons.First().Ordering.Should().Be(_leavingReasonCategory1Reason1.Ordering);
//        category1.Category.Should().Be(_leavingReasonCategory1Reason1.Category);
//    }

//    [Test]
//    public void WhenLeavingReasons_ProcessedResult_CheckThirdCategory()
//    {
//        var result = (GetLeavingReasonsQueryResult)LeavingReasons;

//        var category3 = result.ProcessedResult.LeavingCategories.Skip(2).First();

//        category3.LeavingReasons.Count.Should().Be(1);

//        category3.LeavingReasons.First().Description.Should().BeEquivalentTo(_leavingReasonCategory3Reason1.Description);
//        category3.LeavingReasons.First().Id.Should().Be(_leavingReasonCategory3Reason1.Id);
//        category3.LeavingReasons.First().Ordering.Should().Be(_leavingReasonCategory3Reason1.Ordering);
//        category3.Category.Should().Be(_leavingReasonCategory3Reason1.Category);
//    }

//    [Test]
//    public void WhenLeavingReasons_ProcessedResult_CheckSecondCategoryName()
//    {
//        var result = (GetLeavingReasonsQueryResult)LeavingReasons;

//        var category2 = result.ProcessedResult.LeavingCategories.Skip(1).First();
//        category2.Category.Should().Be(_leavingReasonCategory2Reason1.Category);
//    }

//    [Test]
//    public void WhenLeavingReasons_ProcessedResult_CheckSecondCategoryLeavingReasons()
//    {
//        var result = (GetLeavingReasonsQueryResult)LeavingReasons;

//        var category2 = result.ProcessedResult.LeavingCategories.Skip(1).First();

//        category2.LeavingReasons.Count.Should().Be(3);

//        category2.LeavingReasons.First().Description.Should().BeEquivalentTo(_leavingReasonCategory2Reason3.Description);
//        category2.LeavingReasons.First().Id.Should().Be(_leavingReasonCategory2Reason3.Id);
//        category2.LeavingReasons.First().Ordering.Should().Be(_leavingReasonCategory2Reason3.Ordering);

//        category2.LeavingReasons.Skip(1).First().Description.Should().BeEquivalentTo(_leavingReasonCategory2Reason1.Description);
//        category2.LeavingReasons.Skip(1).First().Id.Should().Be(_leavingReasonCategory2Reason1.Id);
//        category2.LeavingReasons.Skip(1).First().Ordering.Should().Be(_leavingReasonCategory2Reason1.Ordering);

//        category2.LeavingReasons.Skip(2).First().Description.Should().BeEquivalentTo(_leavingReasonCategory2Reason2.Description);
//        category2.LeavingReasons.Skip(2).First().Id.Should().Be(_leavingReasonCategory2Reason2.Id);
//        category2.LeavingReasons.Skip(2).First().Ordering.Should().Be(_leavingReasonCategory2Reason2.Ordering);
//    }
//}
