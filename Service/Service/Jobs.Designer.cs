namespace Hanbell.AutoReport.Service
{
    partial class Jobs
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.eventLog = new System.Diagnostics.EventLog();
            this.timer = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timer)).BeginInit();
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 10000D;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
            // 
            // Jobs
            // 
            this.ServiceName = "Drizzling";
            ((System.ComponentModel.ISupportInitialize)(this.eventLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timer)).EndInit();

        }

        #endregion

        protected System.Diagnostics.EventLog eventLog;
        protected System.Timers.Timer timer;

    }
}
