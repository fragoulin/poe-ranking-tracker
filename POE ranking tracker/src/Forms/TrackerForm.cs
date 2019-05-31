﻿using PoeRankingTracker.Models;
using PoeRankingTracker.Resources.Translations;
using PoeRankingTracker.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Timers;
using System.Windows.Forms;

namespace PoeRankingTracker
{
    public partial class TrackerForm : Form
    {
        private Point lastPoint;
        private System.Timers.Timer timer = new System.Timers.Timer(Properties.Settings.Default.TimerInterval);
        private TrackerConfiguration configuration;
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public TrackerForm()
        {
            InitializeComponent();
            InitializePosition();
            InitializeProgressEvents();
            SetTimer();
        }

        private void InitializeTranslations()
        {
            Text = Strings.Configuration;
            globalRankLabel.Text = Strings.GlobalRank;
            classRankLabel.Text = Strings.ClassRank;
            deadsAheadLabel.Text = Strings.DeadsAhead;
            showExperienceAheadLabel.Text = Strings.ExperienceAhead;
            showExperienceBehindLabel.Text = Strings.ExperienceBehind;
        }

        private void InitializePosition()
        {
            if (Properties.Settings.Default.TrackerMoved)
            {
                StartPosition = FormStartPosition.Manual;
                Location = Properties.Settings.Default.TrackerLocation;
            }
        }

        private void InitializeProgressEvents()
        {
            Api.Instance.GetEntriesStarted += ProgressStarted;
            Api.Instance.GetEntriesIncremented += ProgressIncremented;
            Api.Instance.GetEntriesEnded += ProgressEnded;
        }

        private void ProgressStarted(object sender, ApiEventArgs args)
        {
            progressBar.Invoke(new MethodInvoker(delegate
            {
                progressBar.Value = 0;
                progressBar.Maximum = args.Value;
            }));
        }

        private void ProgressIncremented(object sender, ApiEventArgs args)
        {
            progressBar.Invoke(new MethodInvoker(delegate
            {
                progressBar.PerformStep();
            }));
        }

        private void ProgressEnded(object sender, ApiEventArgs args)
        {
            progressBar.Invoke(new MethodInvoker(delegate
            {
                progressBar.Value = 0;
            }));
        }

        public void SetConfiguration(TrackerConfiguration configuration)
        {
            this.configuration = configuration;
            InitializeTranslations();
            SetStyles();
            SetComponentsVisibility();
            RetrieveData();
            rankValue.Text = string.Format(CultureInfo.CurrentCulture, "{0:#,0}", configuration.Entry.Rank); // Display rank immediately
        }

        private void SetComponentsVisibility()
        {
            classRankValue.Visible = Properties.Settings.Default.ShowRankByClass;
            classRankLabel.Visible = Properties.Settings.Default.ShowRankByClass;
            classRankValue.Text = Strings.PleaseWait;

            deadsAheadValue.Visible = Properties.Settings.Default.ShowDeadsAhead;
            deadsAheadLabel.Visible = Properties.Settings.Default.ShowDeadsAhead;
            deadsAheadValue.Text = Strings.PleaseWait;

            showExperienceAheadValue.Visible = Properties.Settings.Default.ShowExperienceAhead;
            showExperienceAheadLabel.Visible = Properties.Settings.Default.ShowExperienceAhead;
            showExperienceAheadValue.Text = Strings.PleaseWait;

            showExperienceBehindValue.Visible = Properties.Settings.Default.ShowExperienceBehind;
            showExperienceBehindLabel.Visible = Properties.Settings.Default.ShowExperienceBehind;
            showExperienceBehindValue.Text = Strings.PleaseWait;

            progressBar.Visible = Properties.Settings.Default.ShowProgressBar;
        }

        private void SetStyles()
        {
            Font = configuration.Font;
            ForeColor = configuration.FontColor;
            BackColor = configuration.BackgroundColor;
            progressBar.SetColor(configuration.FontColor);
        }

        private async void RetrieveData()
        {
            logger.Debug("RetrieveData");
            timer?.Stop();

            List<Entry> entries = await Api.Instance.GetEntries(configuration.League.Id, configuration.AccountName, configuration.Entry.Character.Name).ConfigureAwait(true);
            ComputeRank(entries);
            ComputeRankByClass(entries);
            ComputeNumberOfDeadsAhead(entries);
            ComputeExperienceAhead(entries);
            ComputeExperienceBehind(entries);

            timer?.Start();
        }

        private void ComputeRank(List<Entry> entries)
        {
            DisplayRank(CharacterService.Instance.GetRank(entries, Properties.Settings.Default.CharacterName));
        }

        private void DisplayRank(int rank)
        {
            rankValue.Invoke(new MethodInvoker(delegate
            {
                rankValue.Text = string.Format(CultureInfo.CurrentCulture, "{0:#,0}", rank);
            }));
        }

        private void ComputeRankByClass(List<Entry> entries)
        {
            if (Properties.Settings.Default.ShowRankByClass)
            {
                var rank = CharacterService.Instance.GetRankByClass(entries, configuration.Entry);
                DisplayRankByClass(rank);
            }
        }

        private void DisplayRankByClass(int rank)
        {
            if (Properties.Settings.Default.ShowRankByClass)
            {
                classRankValue.Invoke(new MethodInvoker(delegate
                {
                    classRankValue.Text = string.Format(CultureInfo.CurrentCulture, "{0:#,0}", rank);
                }));
            }
        }

        private void ComputeNumberOfDeadsAhead(List<Entry> entries)
        {
            if (Properties.Settings.Default.ShowDeadsAhead)
            {
                var n = CharacterService.Instance.GetNumbersOfDeadsAhead(entries, configuration.Entry);
                DisplayNumberOfDeadsAhead(n);
            }
        }

        private void DisplayNumberOfDeadsAhead(int n)
        {
            if (Properties.Settings.Default.ShowDeadsAhead)
            {
                deadsAheadValue.Invoke(new MethodInvoker(delegate
                {
                    deadsAheadValue.Text = string.Format(CultureInfo.CurrentCulture, "{0:#,0}", n);
                }));
            }
        }

        private void ComputeExperienceAhead(List<Entry> entries)
        {
            if (Properties.Settings.Default.ShowExperienceAhead)
            {
                long n = CharacterService.Instance.GetExperienceAhead(entries, configuration.Entry);
                DisplayExperienceAhead(n);
            }
        }

        private void DisplayExperienceAhead(long n)
        {
            if (Properties.Settings.Default.ShowExperienceAhead)
            {
                showExperienceAheadValue.Invoke(new MethodInvoker(delegate
                {
                    showExperienceAheadValue.Text = string.Format(CultureInfo.CurrentCulture, "{0:#,0}", n);
                }));
            }
        }

        private void ComputeExperienceBehind(List<Entry> entries)
        {
            if (Properties.Settings.Default.ShowExperienceBehind)
            {
                long n = CharacterService.Instance.GetExperienceBehind(entries, configuration.Entry);
                DisplayExperienceBehind(n);
            }
        }

        private void DisplayExperienceBehind(long n)
        {
            if (Properties.Settings.Default.ShowExperienceBehind)
            {
                showExperienceBehindValue.Invoke(new MethodInvoker(delegate
                {
                    showExperienceBehindValue.Text = string.Format(CultureInfo.CurrentCulture, "{0:#,0}", n);
                }));
            }
        }

        private void SetTimer()
        {
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            RetrieveData();
        }

        private void TrackerForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Left += e.X - lastPoint.X;
                Top += e.Y - lastPoint.Y;
            }
        }

        private void TrackerForm_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void TrackerForm_MouseDoubleClick(object sender, System.EventArgs e)
        {
            timer.Stop();
            RankingTrackerContext.currentContext.ShowConfigurationForm();
        }

        private void TrackerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
            Properties.Settings.Default.TrackerLocation = Location;
        }

        private void TrackerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void TrackerForm_Move(object sender, System.EventArgs e)
        {
            Properties.Settings.Default.TrackerMoved = true;
        }

        private void TrackerForm_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible)
            {
                timer.Stop();
                Api.Instance.CancelTasks();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();

                timer.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}