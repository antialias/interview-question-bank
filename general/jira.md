# Jira

## lifecycle of a jira ticket
1. **Open** and assigned to an engineer
 - The engineer is assigned and has not started working on it yet.
1. **In Progress**
 - The engineer has begun working on this ticket. The engineer should move it to "In Progress"
1. **Ready For UAT** and assigned to a product manager
 - UAT = User Acceptance Testing
 - The engineer has finished working on this ticket in their topic branch. It is now time for the product manager to UAT the ticket.
 - The engineer will update the ticket status to "Ready For UAT" and assign to the relevant product manager. The engineer will inform the product manager that this work is ready for uat.
 - The engineer will also open a Pull Request so code review can begin
 - The work will be be reviewed by the product manager either on a fruit server or locally on the engineer's [devbox](../front-end/devbox.md). The product manager is looking to ensure that the feature has been implemented as described.
 - **IMPORTANT** This step is always skipped for bugs and may be skipped for tiny changes
1. **In Review** and assigned to the engineer
 - After UAT is complete, the product manager will assign the ticket back to the engineer implementing the feature. 
 - The engineer will then move the ticket to "In Review" until code review is complete and the code is merged.
 - The ticket will stay assigned to the engineer who implemented the feature, not the one doing the code review
1. **Ready For QA** and assigned to a QA engineer
 - After the code is merged, the engineer will update the ticket to "Ready For QA" and assign the ticket to a QA engineer.
 - This step may be skipped if product and engineer decide to release the feature without QA support
1. **Reopened** and assigned to an engineer
 - If a bug is found relating to a specific ticket, it is assigned back to the engineer and the status is set to "Reopened."
 - The engineer will fix the bugs and assign the ticket to QA and update the status to "Ready For QA" **after** bug fixes are merged to the proper branch.
1. **Ready For Release** and assigned to a product manager.
 - When QA and/or testing is complete the ticket's status to set to Ready For Release and it's assigned to the product manager.
1. **Closed**
 - After the feature has been released, the product manager will verify on production and close the ticket.
