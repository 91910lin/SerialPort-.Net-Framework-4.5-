## TABLE_TEST

##### 使用 .Net Framework 4.5 實作的 Serial Port 讀寫程式

##### SerialPort application for .Net Framework 4.5(I developed to get the angle of the company's operating table.)

### 用途

---

**讀取 Serial Port 數據**

> 讀取 Serial Port 數據，數據內容為 16 進制(Ex :FF 07 86 B2 B1 B2 7F 7F 7F 87 F0)

**發送 Serial Port 數據**

> 發送 Serial Port 數據，數據內容為 16 進制

```c#
{
	public static readonly byte[] Get_Status = { 0xFF, 0x01, 0x80, 0x81, 0xF0 };
}
```

> 設置一個 Timer，以固定頻率發送 Serial Port 數據

```c#
{
	private System.Windows.Forms.Timer timer;
	timer = new System.Windows.Forms.Timer();
	timer.Interval = inputValue;
	timer.Tick += Timer_Tick;
	timer.Start();
}
```

**.gitignore**

> 設定 git 的忽略資料夾
