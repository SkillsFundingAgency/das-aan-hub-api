﻿using NUnit.Framework;
using SFA.DAS.AANHub.Application.StagedApprentices.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.StagedApprentices.Queries;

public class GetStagedApprenticeQueryResultTest
{
    [Test, MoqAutoData]
    public void StagedApprentice_ReturnsExpectedFields(StagedApprentice stagedApprentice)
    {
        var response = (GetStagedApprenticeQueryResult?)stagedApprentice;

        Assert.Multiple(() =>
        {
            Assert.That(response, Is.Not.Null);

            Assert.AreEqual(stagedApprentice.Uln, response!.Uln);
            Assert.AreEqual(stagedApprentice.ApprenticeshipId, response.ApprenticeshipId);
            Assert.AreEqual(stagedApprentice.EmployerName, response.EmployerName);
            Assert.AreEqual(stagedApprentice.StartDate, response.StartDate);
            Assert.AreEqual(stagedApprentice.EndDate, response.EndDate);
            Assert.AreEqual(stagedApprentice.TrainingProviderId, response.TrainingProviderId);
            Assert.AreEqual(stagedApprentice.TrainingProviderName, response.TrainingProviderName);
            Assert.AreEqual(stagedApprentice.TrainingCode, response.TrainingCode);
            Assert.AreEqual(stagedApprentice.StandardUId, response.StandardUId);
        });
    }

    [Test, MoqAutoData]
    public void StagedApprentice_GetStagedApprentice_StagedApprenticeIsNull()
    {
        StagedApprentice? stagedApprentice = null;
        var response = (GetStagedApprenticeQueryResult?)stagedApprentice!;
        Assert.That(response, Is.Null);
    }
}
