using DebugTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SupercapController
{
    partial class FormMain
    {
        CommandFormerClass com;

        private void buttonDebugResetInstructions_Click(object sender, EventArgs e)
        {
            com = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            textBoxDebugInstructionPool.Text = "";
            FormCustomConsole.WriteLine("------- Commands reset --------\r\n");
        }

        private void buttonDebugExecute_Click(object sender, EventArgs e)
        {
            var data = com.GetFinalCommandList();
            labelDebugBytesUsed.Text = "Bytes Used : " + data.Length;
            try
            {
                SerialDriver.Send(data, DebugExecuteSuccessCallback, DebugExecuteFailCallback);
            }
            catch (Exception)
            {
                MessageBox.Show("Problem occured while trying to send data to serial port!");
                FormCustomConsole.WriteLine("------- Commands not sent --------\r\n");
                return;

            }
            // Reset everything
            com = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            textBoxDebugInstructionPool.Text = "";
            FormCustomConsole.WriteLine("------- Commands sent --------\r\n");
        }

        private void DebugExecuteSuccessCallback(byte[] b)
        {
            // Its ACK no need to use data
            MessageBox.Show("Commands sent successfully!");
        }

        private void DebugExecuteFailCallback()
        {
            MessageBox.Show("Commands not received by device!");
        }

        private void buttonDebugDataRecTask_Click(object sender, EventArgs e)
        {
            byte ch;
            byte op;
            uint prescaler;
            uint targetPoints;

            // Parse input fields
            try
            {
                ch = InputValidatorHelperClass.GetChModeFromComboBox(comboBoxDebugDataRecTaskCh);
                op = InputValidatorHelperClass.GetOperationModeFromComboBox(comboBoxDebugDataRecTaskOpMode);
                prescaler = Convert.ToUInt32(textBoxDebugPrescaler.Text);
                targetPoints = Convert.ToUInt32(textBoxDebugTargetPoints.Text);

            }
            catch (Exception)
            {
                MessageBox.Show("Wrong Values for Data Recorder!");
                return;
            }

            // Values are valid, add command to the list
            com.AppendDataRecorderTask(ch, op, prescaler, targetPoints, DateTime.Now);

            // Append to display
            textBoxDebugInstructionPool.Text += "DataRecTask(" + comboBoxDebugDataRecTaskCh.Text + ", " + comboBoxDebugDataRecTaskOpMode.Text + " ," + prescaler +
                ", " + targetPoints + ") \r\n";
            FormCustomConsole.WriteLine("DataRecTask(" + comboBoxDebugDataRecTaskCh.Text + ", " + comboBoxDebugDataRecTaskOpMode.Text + " ," + prescaler +
                ", " + targetPoints + ")");

        }

        private void buttonDebugWaitDataRecToFinish_Click(object sender, EventArgs e)
        {
            com.AppendWaitForDataRecorderToFinish();
            textBoxDebugInstructionPool.Text += "Wair for data recorder to finish\r\n";
            FormCustomConsole.WriteLine("Wair for data recorder to finish");
        }

        private void buttonDebugWaitForMs_Click(object sender, EventArgs e)
        {
            UInt32 ms;
            try
            {
                ms = Convert.ToUInt32(textBoxDebugWaitInMiliseconds.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Insert valid number for delay!");
                return;
            }

            com.AppendWaitForMs(ms);
            textBoxDebugInstructionPool.Text += "Delay in ms: " + ms.ToString() + "\r\n";
            FormCustomConsole.WriteLine("Delay in ms: " + ms.ToString());
        }

        private void buttonDebugDataRecFinish_Click(object sender, EventArgs e)
        {
            com.AppendDataRecFinish();
            textBoxDebugInstructionPool.Text += "Data recorder finish (continious mode)\r\n";
            FormCustomConsole.WriteLine("Data recorder finish (continious mode)");
        }

        private void buttonDebugWaitForValueRising_Click(object sender, EventArgs e)
        {
            byte ch;
            UInt16 latency = 0;
            float value, gain;
            
            if (checkBoxDebugUseDefGain.Checked)
            {
                // Use gain that is inferred from configuration file
                try
                {
                    ch = InputValidatorHelperClass.GetChModeFromComboBox(comboBoxDebugWaitForValueRising);
                    //latency = Convert.ToUInt16(textBoxDebugWaitForValueRisingLatency.Text);
                    string corectedValue = textBoxDebugWaitForValueRisingValue.Text;
                    string correctedGain = textBoxDebugWaitForValueRisingGain.Text;
                    value = float.Parse(corectedValue);
                    if (ch == 0)
                    {
                        gain = ConfigClass.deviceGainCH0;
                    }
                    else
                    {
                        gain = ConfigClass.deviceGainCH1;
                    }
                    value = value / gain;
                }
                catch (Exception)
                {
                    MessageBox.Show("Insert valid values!");
                    return;
                }
            }
            else
            {
                // Use gain that is parsed from textbox
                try
                {
                    ch = InputValidatorHelperClass.GetChModeFromComboBox(comboBoxDebugWaitForValueRising);
                    //latency = Convert.ToUInt16(textBoxDebugWaitForValueRisingLatency.Text);
                    string corectedValue = textBoxDebugWaitForValueRisingValue.Text;
                    string correctedGain = textBoxDebugWaitForValueRisingGain.Text;
                    value = float.Parse(corectedValue);
                    gain = float.Parse(correctedGain);
                    value = value / gain;
                }
                catch (Exception)
                {
                    MessageBox.Show("Insert valid values!");
                    return;
                }
            }

            
            com.AppendWaitForValueRising(ch, latency, value);
            textBoxDebugInstructionPool.Text += "WaitForValueRising(" + comboBoxDebugWaitForValueRising.Text + ", " + latency +
                ", " + value.ToString() + ")\r\n";
            FormCustomConsole.WriteLine("WaitForValueRising(" + comboBoxDebugWaitForValueRising.Text + ", " + latency +
                ", " + value.ToString() + ")");
        }

        private void buttonDebugWaitForValueFalling_Click(object sender, EventArgs e)
        {
            byte ch;
            UInt16 latency = 0;
            float value, gain;
            
            if (checkBoxDebugUseDefGain.Checked)
            {
                // Use gain that is inferred from configuration file
                try
                {
                    ch = InputValidatorHelperClass.GetChModeFromComboBox(comboBoxDebugWaitForValueFalling);
                    string corectedValue = textBoxDebugWaitForValueFallingValue.Text;
                    string correctedGain = textBoxDebugWaitForValueFallingGain.Text;
                    value = float.Parse(corectedValue);
                    if (ch == 0)
                    {
                        gain = ConfigClass.deviceGainCH0;
                    }
                    else
                    {
                        gain = ConfigClass.deviceGainCH1;
                    }
                    value = value / gain;
                }
                catch (Exception)
                {
                    MessageBox.Show("Insert valid values!");
                    return;
                }
            }
            else
            {
                // Use gain that is parsed from textbox
                try
                {
                    ch = InputValidatorHelperClass.GetChModeFromComboBox(comboBoxDebugWaitForValueFalling);
                    string corectedValue = textBoxDebugWaitForValueFallingValue.Text;
                    string correctedGain = textBoxDebugWaitForValueFallingGain.Text;
                    value = float.Parse(corectedValue);
                    gain = float.Parse(correctedGain);
                    value = value / gain;
                }
                catch (Exception)
                {
                    MessageBox.Show("Insert valid values!");
                    return;
                }
            }
            
            com.AppendWaitForValueFalling(ch, latency, value);
            textBoxDebugInstructionPool.Text += "WaitForValueFalling(" + comboBoxDebugWaitForValueFalling.Text + ", " + latency +
                ", " + value.ToString() + ")\r\n";
            FormCustomConsole.WriteLine("WaitForValueFalling(" + comboBoxDebugWaitForValueFalling.Text + ", " + latency +
                ", " + value.ToString() + ")");
        }

        private void buttonDebugSetCriticalLow_Click(object sender, EventArgs e)
        {
            byte ch;
            float value, gain;

            if (checkBoxDebugUseDefGain.Checked)
            {
                // Use gain that is inferred from configuration file
                try
                {
                    ch = InputValidatorHelperClass.GetChModeFromComboBox(comboBoxDebugSetCritLow);
                    value = float.Parse(textBoxDebugSetCriticalLow.Text);
                    if (ch == 0)
                    {
                        gain = ConfigClass.deviceGainCH0;
                    }
                    else
                    {
                        gain = ConfigClass.deviceGainCH1;
                    }
                    value = value / gain;
                }
                catch (Exception)
                {
                    MessageBox.Show("Insert valid values!");
                    return;
                }
            }
            else
            {
                // Use gain that is parsed from textbox
                try
                {
                    ch = InputValidatorHelperClass.GetChModeFromComboBox(comboBoxDebugSetCritLow);
                    value = float.Parse(textBoxDebugSetCriticalLow.Text);
                    gain = float.Parse(textBoxDebugSetCriticalLowGain.Text);
                    value = value / gain;
                }
                catch (Exception)
                {
                    MessageBox.Show("Insert valid values!");
                    return;
                }
            }


            com.AppendSetCriticalLow(value, ch);
            textBoxDebugInstructionPool.Text += "SetCriticalLow(" + value + ", " + comboBoxDebugSetCritLow.Text + ")\r\n";
            FormCustomConsole.WriteLine("SetCriticalLow(" + value + ", " + comboBoxDebugSetCritLow.Text + ")");
        }

        private void buttonDebugDiasbleCriticalLow_Click(object sender, EventArgs e)
        {
            byte ch;
            try
            {
                ch = InputValidatorHelperClass.GetChModeFromComboBox(comboBoxDebugDisableCritLow);
            }
            catch (Exception)
            {
                MessageBox.Show("Insert valid values!");
                return;
            }
            UInt16 temp = 0x8000;
            com.AppendSetCriticalLow(temp, ch);
            textBoxDebugInstructionPool.Text += "DisableCriticalLow(" + comboBoxDebugDisableCritLow.Text + ")\r\n";
            FormCustomConsole.WriteLine("DisableCriticalLow()\r\n");
        }

        private void buttonDebugSetCriticalHigh_Click(object sender, EventArgs e)
        {
            byte ch;
            float value, gain;

            if (checkBoxDebugUseDefGain.Checked)
            {
                // Use gain that is inferred from configuration file
                try
                {
                    ch = InputValidatorHelperClass.GetChModeFromComboBox(comboBoxDebugSetCritHigh);
                    value = float.Parse(textBoxDebugSetCriticalHigh.Text);
                    if (ch == 0)
                    {
                        gain = ConfigClass.deviceGainCH0;
                    }
                    else
                    {
                        gain = ConfigClass.deviceGainCH1;
                    }
                    value = value / gain;
                }
                catch (Exception)
                {
                    MessageBox.Show("Insert valid values!");
                    return;
                }
            }
            else
            {
                // Use gain that is parsed from textbox
                try
                {
                    ch = InputValidatorHelperClass.GetChModeFromComboBox(comboBoxDebugSetCritHigh);
                    value = float.Parse(textBoxDebugSetCriticalHigh.Text);
                    gain = float.Parse(textBoxDebugSetCriticalHighGain.Text);
                    value = value / gain;
                }
                catch (Exception)
                {
                    MessageBox.Show("Insert valid values!");
                    return;
                }
            }
            
            com.AppendSetCriticalHigh(value, ch);
            textBoxDebugInstructionPool.Text += "SetCriticalHigh(" + value + ", " + comboBoxDebugSetCritHigh.Text + ")\r\n";
            FormCustomConsole.WriteLine("SetCriticalHigh(" + value + ", " + comboBoxDebugSetCritHigh.Text + ")");
        }

        private void buttonDebugDiasbleCriticalHigh_Click(object sender, EventArgs e)
        {
            byte ch;
            try
            {
                ch = InputValidatorHelperClass.GetChModeFromComboBox(comboBoxDebugDisableCritHigh);
            }
            catch (Exception)
            {
                MessageBox.Show("Insert valid values!");
                return;
            }
            UInt16 temp = 0x8000;
            com.AppendSetCriticalHigh(temp, ch);
            textBoxDebugInstructionPool.Text += "DisableCriticalHigh(" + comboBoxDebugDisableCritHigh.Text + ")\r\n";
            FormCustomConsole.WriteLine("DisableCriticalHigh()\r\n");
        }

        private void buttonDebugLedOn_Click(object sender, EventArgs e)
        {
            byte ledNum;

            try
            {
                ledNum = Convert.ToByte(textBoxDebugLedOn.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Insert valid value!");
                return;
            }
            com.AppendLedOn(ledNum);
            textBoxDebugInstructionPool.Text += "LedOn(" + ledNum + ")\r\n";
            FormCustomConsole.WriteLine("LedOn(" + ledNum + ")");
        }

        private void buttonDebugLedOff_Click(object sender, EventArgs e)
        {
            byte ledNum;

            try
            {
                ledNum = Convert.ToByte(textBoxDebugLedOff.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Insert valid value!");
                return;
            }
            com.AppendLedOff(ledNum);
            textBoxDebugInstructionPool.Text += "LedOff(" + ledNum + ")\r\n";
            FormCustomConsole.WriteLine("LedOff(" + ledNum + ")");
        }

        private void buttonDebugPinSetHigh_Click(object sender, EventArgs e)
        {
            byte pinNum;

            try
            {
                pinNum = Convert.ToByte(textBoxDebugPinSetHigh.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Insert valid value!");
                return;
            }
            com.AppendPinSetHigh(pinNum);
            textBoxDebugInstructionPool.Text += "PinSetHigh(" + pinNum + ")\r\n";
            FormCustomConsole.WriteLine("PinSetHigh(" + pinNum + ")");
        }

        private void buttonDebugPinSetLow_Click(object sender, EventArgs e)
        {
            byte pinNum;

            try
            {
                pinNum = Convert.ToByte(textBoxDebugPinSetLow.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Insert valid value!");
                return;
            }
            com.AppendPinSetLow(pinNum);
            textBoxDebugInstructionPool.Text += "PinSetLow(" + pinNum + ")\r\n";
            FormCustomConsole.WriteLine("PinSetLow(" + pinNum + ")");
        }

        private void buttonDebugChargerOn_Click(object sender, EventArgs e)
        {
            com.AppendChargerOn();
            textBoxDebugInstructionPool.Text += "ChargerOn\r\n";
            FormCustomConsole.WriteLine("ChargerOn");
        }

        private void buttonDebugChargerOff_Click(object sender, EventArgs e)
        {
            com.AppendChargerOff();
            textBoxDebugInstructionPool.Text += "ChargerOff\r\n";
            FormCustomConsole.WriteLine("ChargerOff");
        }

        private void buttonDebugFastChargerOn_Click(object sender, EventArgs e)
        {
            com.FastChargeOn();
            textBoxDebugInstructionPool.Text += "FastChargerOn\r\n";
            FormCustomConsole.WriteLine("FastChargerOn");
        }

        private void buttonDebugFastChargerOff_Click(object sender, EventArgs e)
        {
            com.FastChargeOff();
            textBoxDebugInstructionPool.Text += "FastChargerOff\r\n";
            FormCustomConsole.WriteLine("FastChargerOff");
        }
        
        private void buttonDebugDischarger100AOn_Click(object sender, EventArgs e)
        {
            com.AppendDischarger100AOn();
            textBoxDebugInstructionPool.Text += "Discharger100AOn\r\n";
            FormCustomConsole.WriteLine("Discharger100AOn");
        }

        private void buttonDebugDischarger100AOffS1_Click(object sender, EventArgs e)
        {
            com.AppendDischarger100AOffS1();
            textBoxDebugInstructionPool.Text += "Discharger100AOff S1\r\n";
            FormCustomConsole.WriteLine("Discharger100AOff S1");
        }

        private void buttonDebugDischarger100AOffS2_Click(object sender, EventArgs e)
        {
            com.AppendDischarger100AOffS2();
            textBoxDebugInstructionPool.Text += "Discharger100AOff S2\r\n";
            FormCustomConsole.WriteLine("Discharger100AOff S2");
        }

        private void buttonDebugDischarger10AOn_Click(object sender, EventArgs e)
        {
            com.AppendDischarger10AOn();
            textBoxDebugInstructionPool.Text += "Discharger10AOn\r\n";
            FormCustomConsole.WriteLine("Discharger10AOn");
        }

        private void buttonDebugDischarger10AOffS1_Click(object sender, EventArgs e)
        {
            com.AppendDischarger10AOffS1();
            textBoxDebugInstructionPool.Text += "Discharger10AOff S1\r\n";
            FormCustomConsole.WriteLine("Discharger10AOff S1");
        }

        private void buttonDebugDischarger10AOffS2_Click(object sender, EventArgs e)
        {
            com.AppendDischarger10AOffS2();
            textBoxDebugInstructionPool.Text += "Discharger10AOff S2\r\n";
            FormCustomConsole.WriteLine("Discharger10AOff S2");
        }

        private void buttonDebugFanoxOn_Click(object sender, EventArgs e)
        {
            com.AppendFanoxOn();
            textBoxDebugInstructionPool.Text += "FanoxOn\r\n";
            FormCustomConsole.WriteLine("FanoxOn");
        }

        private void buttonDebugFanoxOff_Click(object sender, EventArgs e)
        {
            com.AppendFanoxOff();
            textBoxDebugInstructionPool.Text += "FanoxOff\r\n";
            FormCustomConsole.WriteLine("FanoxOff");
        }

        private void buttonDebugResOn_Click(object sender, EventArgs e)
        {
            com.AppendResOn();
            textBoxDebugInstructionPool.Text += "ResOn\r\n";
            FormCustomConsole.WriteLine("ResOn");
        }

        private void buttonDebugResOff_Click(object sender, EventArgs e)
        {
            com.AppendResOff();
            textBoxDebugInstructionPool.Text += "ResOff\r\n";
            FormCustomConsole.WriteLine("ResOff");
        }

        private void buttonDebugRequestACK_Click(object sender, EventArgs e)
        {
            com.ReturnACK();
            textBoxDebugInstructionPool.Text += "Return ACK\r\n";
            FormCustomConsole.WriteLine("Return ACK");
        }
        
        private void buttonDebugCompositeFinishDisch10A_Click(object sender, EventArgs e)
        {
            UInt32 ms;
            try
            {
                ms = Convert.ToUInt32(textBoxDebugCompositeMsDelay.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Insert valid number for delay!");
                return;
            }

            com.AppendDischarger10AOffS1();
            textBoxDebugInstructionPool.Text += "Discharger10AOff S1\r\n";
            FormCustomConsole.WriteLine("Discharger10AOff S1");

            com.AppendWaitForMs(ms);
            textBoxDebugInstructionPool.Text += "Delay in ms: " + ms.ToString() + "\r\n";
            FormCustomConsole.WriteLine("Delay in ms: " + ms.ToString());

            com.AppendDischarger10AOffS2();
            textBoxDebugInstructionPool.Text += "Discharger10AOff S2\r\n";
            FormCustomConsole.WriteLine("Discharger10AOff S2");
        }

        private void buttonDebugCompositeFinishDisch100A_Click(object sender, EventArgs e)
        {
            UInt32 ms;
            try
            {
                ms = Convert.ToUInt32(textBoxDebugCompositeMsDelay.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Insert valid number for delay!");
                return;
            }

            com.AppendDischarger100AOffS1();
            textBoxDebugInstructionPool.Text += "Discharger100AOff S1\r\n";
            FormCustomConsole.WriteLine("Discharger100AOff S1");

            com.AppendWaitForMs(ms);
            textBoxDebugInstructionPool.Text += "Delay in ms: " + ms.ToString() + "\r\n";
            FormCustomConsole.WriteLine("Delay in ms: " + ms.ToString());

            com.AppendDischarger100AOffS2();
            textBoxDebugInstructionPool.Text += "Discharger100AOff S2\r\n";
            FormCustomConsole.WriteLine("Discharger100AOff S2");
        }

        private void buttonMiksa_Click(object sender, EventArgs e)
        {
            //buttonDebugDischarger100AOn_Click(this, EventArgs.Empty);
            //buttonDebugWaitForMs_Click(this, EventArgs.Empty);
            //buttonDebugCompositeFinishDisch100A_Click(this, EventArgs.Empty);
            buttonDebugRequestACK_Click(this, EventArgs.Empty);
            buttonDebugLedOn_Click(this, EventArgs.Empty);
            buttonDebugWaitForMs_Click(this, EventArgs.Empty);
            buttonDebugLedOff_Click(this, EventArgs.Empty);

            buttonDebugWaitForMs_Click(this, EventArgs.Empty);
            buttonDebugLedOn_Click(this, EventArgs.Empty);
            buttonDebugWaitForMs_Click(this, EventArgs.Empty);
            buttonDebugLedOff_Click(this, EventArgs.Empty);

            buttonDebugWaitForMs_Click(this, EventArgs.Empty);
            buttonDebugLedOn_Click(this, EventArgs.Empty);
            buttonDebugWaitForMs_Click(this, EventArgs.Empty);
            buttonDebugLedOff_Click(this, EventArgs.Empty);
        }

        private void buttonTestRunDown10A_Click(object sender, EventArgs e)
        {
            buttonDebugDischarger10AOn_Click(this, EventArgs.Empty);
            buttonDebugWaitForValueFalling_Click(this, EventArgs.Empty);
            buttonDebugCompositeFinishDisch10A_Click(this, EventArgs.Empty);
        }

        private void buttonTestRunDown100A_Click(object sender, EventArgs e)
        {
            buttonDebugDischarger100AOn_Click(this, EventArgs.Empty);
            buttonDebugWaitForValueFalling_Click(this, EventArgs.Empty);
            buttonDebugCompositeFinishDisch100A_Click(this, EventArgs.Empty);
        }

        private void buttonTestRunUp_Click(object sender, EventArgs e)
        {
            buttonDebugChargerOn_Click(this, EventArgs.Empty);
            buttonDebugWaitForValueRising_Click(this, EventArgs.Empty);
            buttonDebugFastChargerOff_Click(this, EventArgs.Empty);
        }
    }
}
