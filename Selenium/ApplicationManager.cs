﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Selenium.Helpers;

namespace Selenium;

public class ApplicationManager : IDisposable
{
    private IDictionary<string, object> Vars { get; }
    private StringBuilder _verificationErrors;
    private const string BaseUrl = "https://todoist.com/";
    private static ThreadLocal<ApplicationManager> _app = new();
    public IWebDriver Driver { get; }
    public NavigationHelper NavigationHelper { get; }
    public LoginHelper LoginHelper { get; }
    public TaskHelper TaskHelper { get; }
    public ProjectHelper ProjectHelper { get; }
    public IJavaScriptExecutor JavaScriptExecutor { get; }

    public ApplicationManager()
    {
        Driver = new ChromeDriver();
        Driver.Manage().Window.Maximize();
        JavaScriptExecutor = (IJavaScriptExecutor) Driver;
        Vars = new Dictionary<string, object>();
        _verificationErrors = new StringBuilder();

        NavigationHelper = new NavigationHelper(this, BaseUrl);
        LoginHelper = new LoginHelper(this);
        TaskHelper = new TaskHelper(this);
        ProjectHelper = new ProjectHelper(this);
    }

    public static ApplicationManager GetInstance()
    {
        if (!_app.IsValueCreated)
        {
            var newInstance = new ApplicationManager();
            newInstance.NavigationHelper.OpenHomePage();
            _app.Value = newInstance;
        }

        return _app.Value!;
    }

    public void Dispose()
    {
        try
        {
            Driver.Quit();
        }
        catch (Exception)
        {
            // ignored
        }
    }
}