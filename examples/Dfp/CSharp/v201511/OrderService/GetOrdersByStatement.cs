// Copyright 2015, Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Api.Ads.Dfp.Lib;
using Google.Api.Ads.Dfp.Util.v201511;
using Google.Api.Ads.Dfp.v201511;

using System;

namespace Google.Api.Ads.Dfp.Examples.CSharp.v201511 {
  /// <summary>
  /// This code example gets all orders for a given advertiser. To create orders, run
  /// CreateOrders.cs. To determine which companies are advertisers,
  /// run GetCompaniesByStatement.cs.
  /// </summary>
  public class GetOrdersByStatement : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This code example gets all orders for a given advertiser. To create orders, run " +
            "CreateOrders.cs. To determine which companies are advertisers,run " +
            "GetCompaniesByStatement.cs.";
      }
    }

    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    public static void Main() {
      GetOrdersByStatement codeExample = new GetOrdersByStatement();
      Console.WriteLine(codeExample.Description);
      codeExample.Run(new DfpUser());
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    /// <param name="user">The DFP user object running the code example.</param>
    public void Run(DfpUser user) {
      // Get the OrderService.
      OrderService orderService = (OrderService) user.GetService(DfpService.v201511.OrderService);

      // Set the name of the advertiser (company) to get orders for.
      String advertiserId = _T("INSERT_ADVERTISER_COMPANY_ID_HERE");

      // Create a statement to only select orders for a given advertiser.
      StatementBuilder statementBuilder = new StatementBuilder()
          .Where("advertiserId = :advertiserId")
          .OrderBy("id ASC")
          .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
          .AddValue("advertiserId", advertiserId);

      // Set default for page.
      OrderPage page = new OrderPage();

      try {
        do {
          // Get orders by statement.
          page = orderService.getOrdersByStatement(statementBuilder.ToStatement());

          if (page.results != null && page.results.Length > 0) {
            int i = page.startIndex;
            foreach (Order order in page.results) {
              Console.WriteLine("{0}) Order with ID = '{1}', name = '{2}', and advertiser " +
                  "ID = '{3}' was found.", i, order.id, order.name, order.advertiserId);
              i++;
            }
          }
          statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
        } while(statementBuilder.GetOffset() < page.totalResultSetSize);
        Console.WriteLine("Number of results found: " + page.totalResultSetSize);
      } catch (Exception e) {
        Console.WriteLine("Failed to get orders by statement. Exception says \"{0}\"",
            e.Message);
      }
    }
  }
}
