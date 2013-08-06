using JhLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace JhMessageBoxUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TC01_NotifyUser_VerifyTextWithinSummaryAndDetail()
        {
            using (var testMessageBox = JhMessageBox.BeginTest())
            {
                // Emulate the action of the user selecting "Cancel".
                testMessageBox.TestFacility.SetButtonToSelect(JhDialogResult.Cancel);

                // Show the message-box.
                var options = new JhMessageBoxOptions(JhMessageBoxType.UserMistake);
                options.SummaryText = "Oh geez";
                options.DetailText = "detail text";
                options.ButtonFlags = JhMessageBoxButtons.Ok | JhMessageBoxButtons.Cancel;

                var r = testMessageBox.NotifyUser(options);

                // Check that it displayed okay..
                Assert.AreEqual(JhDialogResult.Cancel, r, "The JhMessageBoxWindow failed to return the correct result!");
                testMessageBox.TestFacility.AssertAMessageBoxHasBeenShown();
                testMessageBox.TestFacility.AssertWasWithinDetailText("detail");
                testMessageBox.TestFacility.AssertWasWithinSummaryText("geez");
            }
        }

        [TestMethod]
        public void TC02_NotifyUserOfMistake_EmulateClickedOk()
        {
            using (var testMessageBox = JhMessageBox.BeginTest())
            {
                // Emulate the action of the user selecting "Cancel".
                testMessageBox.TestFacility.SetButtonToSelect(JhDialogResult.Ok);

                string summaryText = "basic summary text";
                string detailText = "detailed text";

                // Show the message-box.
                var r = testMessageBox.NotifyUserOfMistake(summaryText, detailText);

                // Check that it displayed okay..
                Assert.AreEqual(JhDialogResult.Ok, r, "The JhMessageBoxWindow failed to return the correct result!");
                testMessageBox.TestFacility.AssertAMessageBoxHasBeenShown();
                testMessageBox.TestFacility.AssertWasWithinSummaryText("sum");
                testMessageBox.TestFacility.AssertWasWithinDetailText("detail");
            }
        }

        [TestMethod]
        public void TC03_NotifyUserOfMistake_EmulateTimedOut()
        {
            using (var testMessageBox = JhMessageBox.BeginTest())
            {
                // Emulate the action of the user making no selection.

                string summaryText = "basic summary text";
                string detailText = "detailed text";

                // Show the message-box.
                var r = testMessageBox.NotifyUserOfMistake(summaryText, detailText);

                // Check that it displayed okay..
                Assert.AreEqual(JhDialogResult.TimedOut, r, "The JhMessageBoxWindow failed to return the correct result!");
                testMessageBox.TestFacility.AssertAMessageBoxHasBeenShown();
                testMessageBox.TestFacility.AssertWasWithinSummaryText("summary");
                testMessageBox.TestFacility.AssertWasWithinDetailText("detail");
            }
        }

        [TestMethod]
        public void TC04_NotifyUserAsync_SimpleCall_CorrectResults()
        {
            using (var testMessageBox = JhMessageBox.BeginTest())
            {
                string summaryText = "basic summary text";
                testMessageBox.TestFacility.SetToWaitForExplicitClose();

                // Show the message-box.
                testMessageBox.NotifyUserAsync(summaryText);

                testMessageBox.TestFacility.AssertAMessageBoxIsBeingShown();
                testMessageBox.TestFacility.AssertOkButtonIsPresent();

                testMessageBox.TestFacility.SimulateClosing(JhDialogResult.Ok);

                testMessageBox.TestFacility.AssertAMessageBoxHasBeenShown();
            }
        }

        [TestMethod]
        public void TC05_NotifyUser_CustomButton1_CorrectResults()
        {
            using (var testMessageBox = JhMessageBox.BeginTest())
            {
                var options = new JhMessageBoxOptions()
                    .SetButtonFlags(JhMessageBoxButtons.Yes | JhMessageBoxButtons.No | JhMessageBoxButtons.Cancel)
                    .SetButtonText(JhDialogResult.Yes, "Absolutely!")
                    .SetSummaryText("The Summary Text")
                    .SetIsAsynchronous(true);

                // We're going to test the event mechanism as well.
                options.Completed += new System.EventHandler<MessageBoxCompletedArgs>(OnCompleted);
                _isCompletedEventRaised = false;
                // Set this to any value other than what we are going to expect it to be.
                _theResult = JhDialogResult.Ignore;

                testMessageBox.TestFacility.SetToWaitForExplicitClose();

                // Show the message-box.
                testMessageBox.NotifyUser(options);

                testMessageBox.TestFacility.AssertAMessageBoxIsBeingShown();
                testMessageBox.TestFacility.AssertNoButtonIsPresent();
                testMessageBox.TestFacility.AssertCancelButtonIsPresent();

                testMessageBox.TestFacility.AssertButtonIsPresent("Absolutely!");

                testMessageBox.TestFacility.SimulateClosing(JhDialogResult.Ok);

                testMessageBox.TestFacility.AssertAMessageBoxHasBeenShown();

                Assert.IsTrue(_isCompletedEventRaised, "Why did the Completed event not get raised?");
                Assert.AreEqual(JhDialogResult.Ok, _theResult, "The result did not get set correctly!");
            }
        }

        private bool _isCompletedEventRaised;
        private JhDialogResult _theResult;

        private void OnCompleted(object sender, MessageBoxCompletedArgs e)
        {
            _isCompletedEventRaised = true;
            _theResult = e.Result;
        }

        [TestCleanup]
        public void CleanupYoMess()
        {
            // This prevents that "COM object that has been separated from its underlying RCW cannot be used" error.
            System.Windows.Threading.Dispatcher.CurrentDispatcher.InvokeShutdown();
        }
    }
}
