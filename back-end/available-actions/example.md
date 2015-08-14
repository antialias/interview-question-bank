
##Updating shared-service Model
The first thing you will need to do is to update your model in shared-service to be action driven.

```java
public class Item implements ServiceData, ActionDriven
{
  // ...
  private List<Action> actions;
  //...
  
  public void setActions(List<Action> actions)
  {
    this.actions = actions;
  }
  
  public List<Action> getActions()
  {
    return actions;
  }
  
  //...
}
```

##Adding Custom Action Processor
If you are trying to add an action that does not relate to a state transition, create an ActionProcessor that contains the 
for that action. IMPORTANT: Make sure it is either marked as a component or defined as a bean in some spring xml file. There needs to be an instance of the class in your application context.

```java
import com.dibs.lib.action.ActionProcessor

@Component
public class FooActionProcessor implements ActionProcessor<DomainItem>
{
  public Action process(DomainItem item)
  {
    Boolean foo = Boolean.TRUE;
    Action action = null;
    
    if(foo)
    {
      action = buildAction()
    }
    
    return action;
  }
  
  private Action buildAction()
  {
    //These could be built up in a much smarter way, but for this example...
    Action action = new CustomAction();
    action.setCode("FOO");
    
    AuthorizedRole role = new AdminRole();
    role.setName("ADMIN");
    
    List<String> permissions = Lists.newArrayList("PERMISSION_THAT_LETS_YOU_DO_FOO");
    
    role.setPermissions(permissions);
    
    action.setAuthorizedRoles(Lists.newArrayList(role));
    
    return action;
  }
}
```

##Workflow
If the model you are using is workflow enabled, actions will automatically be created for transitions. If you do not want a
certain transition to be visible, you can override this by setting the actionDriven property to false on that transition.

####XML
```xml
  <bean id="newToPendingReviewTransition" class="com.dibs.workflow.design.Transition" >
			<property name="transitionName" value="NEW_TO_PENDING" />
			<property name="fromState" ref="REVIEW_STATUS_NEW" />
			<property name="toState" ref="REVIEW_STATUS_PENDING" />
			<property name="actionDriven" value="false" />
			<property name="authorizedRoles">
				<set value-type="com.dibs.inventory.review.workflow.ReviewServiceWorkflow.ReviewServicePartyRole">
	                <value>IMAGE_ADMIN</value>
	        	</set>
			</property>
		</bean>
```

####Java
```java
.addTransition(new Transition<DomainBatchItemProcess, DomainBatchItemProcessStatus>(FullServiceBatchItemWorkflowTransitions.COMPLETE.toString())
						.setFromState(DomainBatchItemProcessStatus.ITEM_BATCH_STARTED)
						.setToState(DomainBatchItemProcessStatus.ITEM_BATCH_SUCCEEDED)
						.setValidationFunction(null)
						.setActionDriven(false);
						.setPostTransitionFunction(fullServiceBatchItemWorkflow.COMPLETE_POST_TRANSITION_FN)
						.setAuthorizedRoles(FullServiceRole.IMAGE_TEAM))
```


