using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NUnit.Framework;
using OpenQA.Selenium;
using ProjectMars_OnboardTask2.Pages;
using ProjectMars_OnboardTask2.Utilities;
using qa_dotnet_cucumber.Helpers;
using Reqnroll;

[Binding]
public class SignInSteps
 
{

    private readonly NavigationHelper _navigationHelper;
    private readonly SignInPage _signInPage;
    

    public SignInSteps (NavigationHelper navigationHelper,SignInPage signinPage)
    {

        _navigationHelper = navigationHelper;
        _signInPage = signinPage;
    }

    [Given(@"I am in the sign in page")]
    public void GivenIAmInTheSignInPage()
    {
        _navigationHelper.NavigateTo(""); 
        _navigationHelper.GoToSignInPage(); 
       
    }

    
    [When("I enter valid {string} and valid {string}")]
    public void WhenIEnterValidAndValid(string email, string password)
    {
       
         _signInPage.SignInSteps(email,password);
    }

  

    [When("I enter valid {string} and valid {string} without selecting Remember me")]
    public void WhenIEnterValidAndValidWithoutSelectingRememberMe(string email, string password)
    {
        _signInPage.SignInSteps(email, password);
    }



    [Then("I should see my profile page with greeting {string}")]
    public void ThenIShouldSeeMyProfilePageWithGreeting(string message)
    {
        //verify profile page is shown

        _signInPage.AssertSuccessfulLogIn(message); 
 
    }

    [Then("I should see my profile page with greeting including my first name within the {string}")]
    public void ThenIShouldSeeMyProfilePageWithGreetingIncludingMyFirstNameWithinThe(string message)
    {
        //verify profile page is shown with greeting message with my first name

        _signInPage.AssertSuccessfulLogIn(message);
    }



    [When("I enter  {string} and  {string}")]
    public void WhenIEnterAnd(string email, string password)
    {
       
        _signInPage.SignInSteps(email, password);
    }



    [Then("an error {string} should appear")]
    public void ThenAnErrorShouldAppear(string message)
    {
        // verify invalid credential error is shown
        _signInPage.AssertFailedLogIn(message);
    }



}