﻿using System;
using System.Linq;
using Marten.Testing.Fixtures;
using Shouldly;

namespace Marten.Testing.Linq
{
    public class invoking_queryable_through_single : DocumentSessionFixture
    {
        public void single_hit_with_only_one_document()
        {
            theSession.Store(new Target{Number = 1});
            theSession.Store(new Target{Number = 2});
            theSession.Store(new Target{Number = 3});
            theSession.Store(new Target{Number = 4});
            theSession.SaveChanges();

            theSession.Query<Target>().Single(x => x.Number == 3)
                .ShouldNotBeNull();
        }

        public void single_or_default_hit_with_only_one_document()
        {
            theSession.Store(new Target { Number = 1 });
            theSession.Store(new Target { Number = 2 });
            theSession.Store(new Target { Number = 3 });
            theSession.Store(new Target { Number = 4 });
            theSession.SaveChanges();

            theSession.Query<Target>().SingleOrDefault(x => x.Number == 3)
                .ShouldNotBeNull();
        }

        public void single_or_default_miss()
        {
            theSession.Store(new Target { Number = 1 });
            theSession.Store(new Target { Number = 2 });
            theSession.Store(new Target { Number = 3 });
            theSession.Store(new Target { Number = 4 });
            theSession.SaveChanges();

            theSession.Query<Target>().SingleOrDefault(x => x.Number == 11)
                .ShouldBeNull();
        }

        public void single_hit_with_more_than_one_match()
        {
            theSession.Store(new Target { Number = 1 });
            theSession.Store(new Target { Number = 2 });
            theSession.Store(new Target { Number = 2 });
            theSession.Store(new Target { Number = 4 });
            theSession.SaveChanges();

            Exception<InvalidOperationException>.ShouldBeThrownBy(() =>
            {
                theSession.Query<Target>().Where(x => x.Number == 2).Single();
            }).Message.ShouldBe("Sequence contains more than one element");
        }

        public void single_or_default_hit_with_more_than_one_match()
        {
            theSession.Store(new Target { Number = 1 });
            theSession.Store(new Target { Number = 2 });
            theSession.Store(new Target { Number = 2 });
            theSession.Store(new Target { Number = 4 });
            theSession.SaveChanges();

            Exception<InvalidOperationException>.ShouldBeThrownBy(() =>
            {
                theSession.Query<Target>().Where(x => x.Number == 2).SingleOrDefault();
            }).Message.ShouldBe("Sequence contains more than one element");
        }

        public void single_miss()
        {
            theSession.Store(new Target { Number = 1 });
            theSession.Store(new Target { Number = 2 });
            theSession.Store(new Target { Number = 3 });
            theSession.Store(new Target { Number = 4 });
            theSession.SaveChanges();

            Exception<InvalidOperationException>.ShouldBeThrownBy(() =>
            {
                theSession.Query<Target>().Where(x => x.Number == 11).Single();
            }).Message.ShouldBe("Sequence contains no elements");
        }
    }
}