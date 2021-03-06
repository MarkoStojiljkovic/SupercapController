﻿using DebugTools;
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

        /// <summary>
        /// Set default values for some controls to reduse agony of filling them over and over on every program restart
        /// </summary>
        private void TabDebugSetDefaultValues()
        {
            comboBoxDebugWaitForValueRising.SelectedIndex = 1;
            comboBoxDebugWaitForValueFalling.SelectedIndex = 1;
            comboBoxDebugSetCritLow.SelectedIndex = 1;
            comboBoxDebugDisableCritLow.SelectedIndex = 1;
            comboBoxDebugSetCritHigh.SelectedIndex = 1;
            comboBoxDebugDisableCritHigh.SelectedIndex = 1;
            textBoxDebugCompositeMsDelay.Text = "200";
        }

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
            labelExecuteStatus.Text = "Pending";
            labelExecuteStatus.ForeColor = System.Drawing.Color.Black;
            // Reset everything
            com = new CommandFormerClass(ConfigClass.startSeq, ConfigClass.deviceAddr);
            textBoxDebugInstructionPool.Text = "";
            FormCustomConsole.WriteLine("------- Commands sent --------\r\n");
        }

        private void DebugExecuteSuccessCallback(byte[] b)
        {
            // Its ACK no need to use data
            //MessageBox.Show("Commands sent successfully!");
            Invoke((MethodInvoker)delegate
            {
                labelExecuteStatus.Text = "Success!";
                labelExecuteStatus.ForeColor = System.Drawing.Color.Green;
            }
            );
            
            FormCustomConsole.WriteLineWithConsole("COMMANDS SENT SUCCESSFULLY!!!");
        }

        private void DebugExecuteFailCallback()
        {
            //MessageBox.Show("Commands not received by device!");
            Invoke((MethodInvoker)delegate
            {
                labelExecuteStatus.Text = "Fail!";
                labelExecuteStatus.ForeColor = System.Drawing.Color.Red;
            });
            FormCustomConsole.WriteLineWithConsole("COMMANDS NOT RECEIVED BY DEVICE!!!");
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
            UInt16 latency;
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

            // parse latency
            try
            {
                latency = Convert.ToUInt16(textBoxDebugWaitForValueRisingLatency.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Insert valid value for latency!");
                return;
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
            UInt16 latency;
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

            // parse latency
            try
            {
                latency = Convert.ToUInt16(textBoxDebugWaitForValueFallingLatency.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Insert valid value for latency!");
                return;
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



        private void buttonDebugSetCutoffValue_Click(object sender, EventArgs e)
        {
            float value = float.Parse(textBoxDebugSetCutoffValue.Text);
            float gain = ConfigClass.deviceGainCH1;
            value = value / gain;
            com.SetCutoffValueCH1(value);

            textBoxDebugInstructionPool.Text += "SetCutoffValue(" + value + ") " + textBoxDebugSetCutoffValue.Text + "\r\n";
            FormCustomConsole.WriteLine("SetCutoffValue(" + value + ")" + textBoxDebugSetCutoffValue.Text);

        }

        private void buttonDebugDisableCutoffValue_Click(object sender, EventArgs e)
        {
            com.SetCutoffValueCH1(0x8000);
            textBoxDebugInstructionPool.Text += "DisableCutoffValue()\r\n";
            FormCustomConsole.WriteLine("DisableCutoffValue()\r\n");
        }




        #region COMPOSITE COMMANDS

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
            float tmpValue;

            // Request return ACK
            com.ReturnACK();
            textBoxDebugInstructionPool.Text += "Return ACK\r\n";
            FormCustomConsole.WriteLine("Return ACK");

            // Discharge 10 On
            com.AppendDischarger10AOn();
            textBoxDebugInstructionPool.Text += "Discharger35AOn\r\n";
            FormCustomConsole.WriteLine("Discharger35AOn");

            // Wait for value falling 2000
            tmpValue = 2000 / ConfigClass.deviceGainCH1; ;
            com.AppendWaitForValueFalling(1, 3, tmpValue);
            textBoxDebugInstructionPool.Text += "WaitForValueFalling(" + "CH1" + ", " + 3 +
                ", " + tmpValue.ToString() + ") 2000  \r\n";
            FormCustomConsole.WriteLine("WaitForValueFalling(" + "CH1" + ", " + 3 +
                ", " + tmpValue.ToString() + ") 2000");

            // Discharger 10A OFF S1
            com.AppendDischarger10AOffS1();
            textBoxDebugInstructionPool.Text += "Discharger35AOff S1\r\n";
            FormCustomConsole.WriteLine("Discharger35AOff S1");

            // Delay 200ms
            com.AppendWaitForMs(200);
            textBoxDebugInstructionPool.Text += "Delay in mseconds: " + 200 + "\r\n";
            FormCustomConsole.WriteLine("Delay in mseconds: " + 200);

            // Discharger 10A OFF S2
            com.AppendDischarger10AOffS2();
            textBoxDebugInstructionPool.Text += "Discharger35AOff S2\r\n";
            FormCustomConsole.WriteLine("Discharger35AOff S2");



            // Charger on
            com.AppendChargerOn();
            textBoxDebugInstructionPool.Text += "ChargerOn\r\n";
            FormCustomConsole.WriteLine("ChargerOn");
            
            // Wait for value rising  16000
            tmpValue = 16000 / ConfigClass.deviceGainCH1; ;

            com.AppendWaitForValueRising(1, 3, tmpValue);
            textBoxDebugInstructionPool.Text += "WaitForValueRising(" + "CH1" + ", " + 3 +
                ", " + tmpValue.ToString() + ") 16000 \r\n";
            FormCustomConsole.WriteLine("WaitForValueRising(" + "CH1" + ", " + 3 +
                ", " + tmpValue.ToString() + ") 16000");

            // Charger off
            com.AppendChargerOff();
            textBoxDebugInstructionPool.Text += "ChargerOff\r\n";
            FormCustomConsole.WriteLine("ChargerOff");
            

            // Data recorder task CH1 , 1- continuous, 0-target points
            com.AppendDataRecorderTask(2, 1, 0, 0, DateTime.Now);
            textBoxDebugInstructionPool.Text += "DataRecTask(" + "CH1" + ", " + "continious" + " ," + "0" +
                ", " + "0" + ") \r\n";
            FormCustomConsole.WriteLine("DataRecTask(" + "CH1" + ", " + "continious" + " ," + "0" +
                ", " + "0" + ")");

            // Delay 15sec   15000 ms
            com.AppendWaitForMs(15000);
            textBoxDebugInstructionPool.Text += "Delay in seconds: 15\r\n";
            FormCustomConsole.WriteLine("Delay in seconds: 15");

            // Discharge 10 On
            com.AppendDischarger10AOn();
            textBoxDebugInstructionPool.Text += "Discharger35AOn\r\n";
            FormCustomConsole.WriteLine("Discharger35AOn");

            // Wait for value falling 8000
            tmpValue = 8000 / ConfigClass.deviceGainCH1; ;
            com.AppendWaitForValueFalling(1, 3, tmpValue);
            textBoxDebugInstructionPool.Text += "WaitForValueFalling(" + "CH1" + ", " + 3 +
                ", " + tmpValue.ToString() + ") 8000  \r\n";
            FormCustomConsole.WriteLine("WaitForValueFalling(" + "CH1" + ", " + 3 +
                ", " + tmpValue.ToString() + ") 8000");

            // Discharger 10A OFF S1
            com.AppendDischarger10AOffS1();
            textBoxDebugInstructionPool.Text += "Discharger35AOff S1\r\n";
            FormCustomConsole.WriteLine("Discharger35AOff S1");

            // Delay 200ms
            com.AppendWaitForMs(200);
            textBoxDebugInstructionPool.Text += "Delay in mseconds: " + 200 + "\r\n";
            FormCustomConsole.WriteLine("Delay in mseconds: " + 200);

            // Discharger 10A OFF S2
            com.AppendDischarger10AOffS2();
            textBoxDebugInstructionPool.Text += "Discharger35AOff S2\r\n";
            FormCustomConsole.WriteLine("Discharger35AOff S2");

            // Delay 5 sec
            com.AppendWaitForMs(5000);
            textBoxDebugInstructionPool.Text += "Delay in seconds: " + 5 + "\r\n";
            FormCustomConsole.WriteLine("Delay in seconds: " + 5);
            
            // Data recorder finish
            com.AppendDataRecFinish();
            textBoxDebugInstructionPool.Text += "Data recorder finish (continious mode)\r\n";
            FormCustomConsole.WriteLine("Data recorder finish (continious mode)");

            // Charger on
            com.AppendChargerOn();
            textBoxDebugInstructionPool.Text += "ChargerOn\r\n";
            FormCustomConsole.WriteLine("ChargerOn");

            // Wait for value rising  16000
            tmpValue = 16000 / ConfigClass.deviceGainCH1; ;

            com.AppendWaitForValueRising(1, 3, tmpValue);
            textBoxDebugInstructionPool.Text += "WaitForValueRising(" + "CH1" + ", " + 3 +
                ", " + tmpValue.ToString() + ") 16000 \r\n";
            FormCustomConsole.WriteLine("WaitForValueRising(" + "CH1" + ", " + 3 +
                ", " + tmpValue.ToString() + ") 16000");

            // Charger off
            com.AppendChargerOff();
            textBoxDebugInstructionPool.Text += "ChargerOff\r\n";
            FormCustomConsole.WriteLine("ChargerOff");

            // Delay 1sec   5000 ms
            com.AppendWaitForMs(5000);
            textBoxDebugInstructionPool.Text += "Delay in seconds: 5\r\n";
            FormCustomConsole.WriteLine("Delay in seconds: 5");


            // Data recorder task DUAL CH, TARGET POINTS, 0 Prescaler, 700 points
            com.AppendDataRecorderTask(2, 2, 0, 700, DateTime.Now);
            textBoxDebugInstructionPool.Text += "DataRecTask(" + "CH1" + ", " + "continious" + " ," + "0" +
                ", " + "0" + ") \r\n";
            FormCustomConsole.WriteLine("DataRecTask(" + "CH1" + ", " + "continious" + " ," + "0" +
                ", " + "0" + ")");

            // Delay 1sec   1000 ms
            com.AppendWaitForMs(1000);
            textBoxDebugInstructionPool.Text += "Delay in seconds: 1\r\n";
            FormCustomConsole.WriteLine("Delay in seconds: 1");
            
            // Res on
            com.AppendResOn();
            textBoxDebugInstructionPool.Text += "ResOn()\r\n";
            FormCustomConsole.WriteLine("ResOn()");

            // Delay 500msec   500 ms
            com.AppendWaitForMs(500);
            textBoxDebugInstructionPool.Text += "Delay in ms: 500\r\n";
            FormCustomConsole.WriteLine("Delay in ms: 500");

            // Res off
            com.AppendResOff();
            textBoxDebugInstructionPool.Text += "ResOff()\r\n";
            FormCustomConsole.WriteLine("ResOff()");
            

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

        private void buttonTestAll_Click(object sender, EventArgs e)
        {

            AppendTestSequence();
        }

        private void AppendTestSequence()
        {
            float tmpValue;

            // Request return ACK
            com.ReturnACK();
            textBoxDebugInstructionPool.Text += "Return ACK\r\n";
            FormCustomConsole.WriteLine("Return ACK");

            // First disable fanox
            com.AppendFanoxOff();
            textBoxDebugInstructionPool.Text += "Fanox off\r\n";
            FormCustomConsole.WriteLine("Fanox off");

            // Data recorder task DUAL CH, TARGET POINTS, 0 Prescaler, 700 points
            com.AppendDataRecorderTask(2, 2, 0, 700, DateTime.Now);
            textBoxDebugInstructionPool.Text += "DataRecTask(" + "CH1" + ", " + "continious" + " ," + "0" +
                ", " + "0" + ") \r\n";
            FormCustomConsole.WriteLine("DataRecTask(" + "CH1" + ", " + "continious" + " ," + "0" +
                ", " + "0" + ")");

            // Delay 1sec   1000 ms
            com.AppendWaitForMs(1000);
            textBoxDebugInstructionPool.Text += "Delay in seconds: 1\r\n";
            FormCustomConsole.WriteLine("Delay in seconds: 1");


            // Res on
            com.AppendResOn();
            textBoxDebugInstructionPool.Text += "ResOn()\r\n";
            FormCustomConsole.WriteLine("ResOn()");

            // Delay 500msec   500 ms
            com.AppendWaitForMs(500);
            textBoxDebugInstructionPool.Text += "Delay in ms: 500\r\n";
            FormCustomConsole.WriteLine("Delay in ms: 500");

            // Res off
            com.AppendResOff();
            textBoxDebugInstructionPool.Text += "ResOff()\r\n";
            FormCustomConsole.WriteLine("ResOff()");

            // Delay 2sec   2 sec
            com.AppendWaitForMs(2000);
            textBoxDebugInstructionPool.Text += "Delay 2sec\r\n";
            FormCustomConsole.WriteLine("Delay 2sec");

            // Discharge 10 On
            com.AppendDischarger10AOn();
            textBoxDebugInstructionPool.Text += "Discharger10AOn\r\n";
            FormCustomConsole.WriteLine("Discharger10AOn");

            // Wait for value falling 2200
            tmpValue = 2200 / ConfigClass.deviceGainCH1; ;
            com.AppendWaitForValueFalling(1, 5, tmpValue);
            textBoxDebugInstructionPool.Text += "WaitForValueFalling(" + "CH1" + ", " + 0 +
                ", " + tmpValue.ToString() + ") 2200  \r\n";
            FormCustomConsole.WriteLine("WaitForValueFalling(" + "CH1" + ", " + 0 +
                ", " + tmpValue.ToString() + ") 2200");
            
            // Discharger 10A OFF S1
            com.AppendDischarger10AOffS1();
            textBoxDebugInstructionPool.Text += "Discharger10AOff S1\r\n";
            FormCustomConsole.WriteLine("Discharger10AOff S1");

            // Delay 200ms
            com.AppendWaitForMs(200);
            textBoxDebugInstructionPool.Text += "Delay in mseconds: " + 200 + "\r\n";
            FormCustomConsole.WriteLine("Delay in mseconds: " + 200);

            // Discharger 10A OFF S2
            com.AppendDischarger10AOffS2();
            textBoxDebugInstructionPool.Text += "Discharger10AOff S2\r\n";
            FormCustomConsole.WriteLine("Discharger10AOff S2");

            // Delay 5 sec
            com.AppendWaitForMs(5000);
            textBoxDebugInstructionPool.Text += "Delay in seconds: " + 5 + "\r\n";
            FormCustomConsole.WriteLine("Delay in seconds: " + 5);

            // ORIGINAL
            // Charger on
            com.AppendChargerOn();
            textBoxDebugInstructionPool.Text += "ChargerOn\r\n";
            FormCustomConsole.WriteLine("ChargerOn");
            
            
            // Wait for value rising  3800
            tmpValue = 3800 / ConfigClass.deviceGainCH1; ;

            com.AppendWaitForValueRising(1, 5, tmpValue);
            textBoxDebugInstructionPool.Text += "WaitForValueRising(" + "CH1" + ", " + 0 +
                ", " + tmpValue.ToString() + ") 3800 \r\n";
            FormCustomConsole.WriteLine("WaitForValueRising(" + "CH1" + ", " + 0 +
                ", " + tmpValue.ToString() + ") 3800");

            // Fast charger off
            com.FastChargeOff();
            textBoxDebugInstructionPool.Text += "FastChargerOff\r\n";
            FormCustomConsole.WriteLine("FastChargerOff");
            

            // Delay 30min   1800000 ms
            com.AppendWaitForMs(1800000);
            textBoxDebugInstructionPool.Text += "Delay in seconds: " + 1800 + "\r\n";
            FormCustomConsole.WriteLine("Delay in seconds: " + 1800);

            // Discharge 10 On
            com.AppendDischarger10AOn();
            textBoxDebugInstructionPool.Text += "Discharger10AOn\r\n";
            FormCustomConsole.WriteLine("Discharger10AOn");
            

            // Delay 1sec   1000 ms
            //com.AppendWaitForMs(1000);
            //textBoxDebugInstructionPool.Text += "Delay in seconds: " + 1 + "\r\n";
            //FormCustomConsole.WriteLine("Delay in seconds: " + 1);

            // Data recorder task CH1 , 1- continuous, 0-target points
            com.AppendDataRecorderTask(2, 1, 0, 0, DateTime.Now);
            textBoxDebugInstructionPool.Text += "DataRecTask(" + "CH1" + ", " + "continious" + " ," + "0" +
                ", " + "0" + ") \r\n";
            FormCustomConsole.WriteLine("DataRecTask(" + "CH1" + ", " + "continious" + " ," + "0" +
                ", " + "0" + ")");

            // Delay 3sec   3000 ms
            com.AppendWaitForMs(3000);
            textBoxDebugInstructionPool.Text += "Delay in seconds: " + 3 + "\r\n";
            FormCustomConsole.WriteLine("Delay in seconds: " + 3);

            // Data recorder finish
            com.AppendDataRecFinish();
            textBoxDebugInstructionPool.Text += "Data recorder finish (continious mode)\r\n";
            FormCustomConsole.WriteLine("Data recorder finish (continious mode)");

            // Delay 3sec   1000 ms
            com.AppendWaitForMs(1000);
            textBoxDebugInstructionPool.Text += "Delay in seconds: " + 1 + "\r\n";
            FormCustomConsole.WriteLine("Delay in seconds: " + 1);
            
            // Data recorder task CH1 , 1- continuous, 0-target points
            com.AppendDataRecorderTask(2, 1, 100, 0, DateTime.Now);
            textBoxDebugInstructionPool.Text += "DataRecTask(" + "CH1" + ", " + "continious" + " ," + "100" +
                ", " + "0" + ") \r\n";
            FormCustomConsole.WriteLine("DataRecTask(" + "CH1" + ", " + "continious" + " ," + "100" +
                ", " + "0" + ")");

            // Wait for value falling 2200
            tmpValue = 2200 / ConfigClass.deviceGainCH1; ;
            com.AppendWaitForValueFalling(1, 5, tmpValue);
            textBoxDebugInstructionPool.Text += "WaitForValueFalling(" + "CH1" + ", " + 0 +
                ", " + tmpValue.ToString() + ") 2200  \r\n";
            FormCustomConsole.WriteLine("WaitForValueFalling(" + "CH1" + ", " + 0 +
                ", " + tmpValue.ToString() + ") 2200");

            // Data recorder finish
            com.AppendDataRecFinish();
            textBoxDebugInstructionPool.Text += "Data recorder finish (continious mode)\r\n";
            FormCustomConsole.WriteLine("Data recorder finish (continious mode)");

            
            // Discharger 10A OFF S1
            com.AppendDischarger10AOffS1();
            textBoxDebugInstructionPool.Text += "Discharger10AOff S1\r\n";
            FormCustomConsole.WriteLine("Discharger10AOff S1");

            // Delay 200ms
            com.AppendWaitForMs(200);
            textBoxDebugInstructionPool.Text += "Delay in mseconds: " + 200 + "\r\n";
            FormCustomConsole.WriteLine("Delay in mseconds: " + 200);

            // Discharger 10A OFF S2
            com.AppendDischarger10AOffS2();
            textBoxDebugInstructionPool.Text += "Discharger10AOff S2\r\n";
            FormCustomConsole.WriteLine("Discharger10AOff S2");
            

            // Delay 1s
            com.AppendWaitForMs(1000);
            textBoxDebugInstructionPool.Text += "Delay in seconds: " + 1 + "\r\n";
            FormCustomConsole.WriteLine("Delay in seconds: " + 1);
            
            // Charger on
            com.AppendChargerOn();
            textBoxDebugInstructionPool.Text += "ChargerOn\r\n";
            FormCustomConsole.WriteLine("ChargerOn");

            // Wait for value rising  3800
            tmpValue = 3800 / ConfigClass.deviceGainCH1; ;

            com.AppendWaitForValueRising(1, 5, tmpValue);
            textBoxDebugInstructionPool.Text += "WaitForValueRising(" + "CH1" + ", " + 0 +
                ", " + tmpValue.ToString() + ")3800 \r\n";
            FormCustomConsole.WriteLine("WaitForValueRising(" + "CH1" + ", " + 0 +
                ", " + tmpValue.ToString() + ") 3800");

            // Fast charger off
            com.FastChargeOff();
            textBoxDebugInstructionPool.Text += "FastChargerOff\r\n";
            FormCustomConsole.WriteLine("FastChargerOff");
            

            // Delay 30min   1800000 ms
            com.AppendWaitForMs(1800000);
            textBoxDebugInstructionPool.Text += "Delay in seconds: " + 1800 + "\r\n";
            FormCustomConsole.WriteLine("Delay in seconds: " + 1800);

            // Charger off
            com.AppendChargerOff();
            textBoxDebugInstructionPool.Text += "ChargerOff\r\n";
            FormCustomConsole.WriteLine("ChargerOff");
            

            // Delay 5min   300000 ms
            com.AppendWaitForMs(300000);
            textBoxDebugInstructionPool.Text += "Delay in seconds: " + 300 + "\r\n";
            FormCustomConsole.WriteLine("Delay in seconds: " + 300);

            // Data recorder task CH1 , 1- continuous, 0-target points
            com.AppendDataRecorderTask(2, 1, 0, 0, DateTime.Now);
            textBoxDebugInstructionPool.Text += "DataRecTask(" + "CH1" + ", " + "continious" + " ," + "0" +
                ", " + "0" + ") \r\n";
            FormCustomConsole.WriteLine("DataRecTask(" + "CH1" + ", " + "continious" + " ," + "0" +
                ", " + "0" + ")");

            // Delay 1s
            com.AppendWaitForMs(1000);
            textBoxDebugInstructionPool.Text += "Delay in seconds: " + 1 + "\r\n";
            FormCustomConsole.WriteLine("Delay in seconds: " + 1);

            // Discharger 100A On
            com.AppendDischarger100AOn();
            textBoxDebugInstructionPool.Text += "Discharger100AOn\r\n";
            FormCustomConsole.WriteLine("Discharger100AOn");

            // Wait for value falling 3000
            tmpValue = 3000 / ConfigClass.deviceGainCH1; ;
            com.AppendWaitForValueFalling(1, 5, tmpValue);
            textBoxDebugInstructionPool.Text += "WaitForValueFalling(" + "CH1" + ", " + 0 +
                ", " + tmpValue.ToString() + ") 3000  \r\n";
            FormCustomConsole.WriteLine("WaitForValueFalling(" + "CH1" + ", " + 0 +
                ", " + tmpValue.ToString() + ") 3000");

            // Discharger 100A OFF S1
            com.AppendDischarger100AOffS1();
            textBoxDebugInstructionPool.Text += "Discharger100AOff S1\r\n";
            FormCustomConsole.WriteLine("Discharger100AOff S1");

            // Delay 200ms
            com.AppendWaitForMs(200);
            textBoxDebugInstructionPool.Text += "Delay in mseconds: " + 200 + "\r\n";
            FormCustomConsole.WriteLine("Delay in mseconds: " + 200);

            // Discharger 100A OFF S2
            com.AppendDischarger100AOffS2();
            textBoxDebugInstructionPool.Text += "Discharger100AOff S2\r\n";
            FormCustomConsole.WriteLine("Discharger100AOff S2");

            // Data recorder finish
            com.AppendDataRecFinish();
            textBoxDebugInstructionPool.Text += "Data recorder finish (continious mode)\r\n";
            FormCustomConsole.WriteLine("Data recorder finish (continious mode)");

            // Delay 1s
            com.AppendWaitForMs(1000);
            textBoxDebugInstructionPool.Text += "Delay in seconds 1\r\n";
            FormCustomConsole.WriteLine("Delay in seconds 1");

            // Charger on
            com.AppendChargerOn();
            textBoxDebugInstructionPool.Text += "ChargerOn\r\n";
            FormCustomConsole.WriteLine("ChargerOn");

            // Wait for value rising  3800
            tmpValue = 3800 / ConfigClass.deviceGainCH1; ;

            com.AppendWaitForValueRising(1, 5, tmpValue);
            textBoxDebugInstructionPool.Text += "WaitForValueRising(" + "CH1" + ", " + 0 +
                ", " + tmpValue.ToString() + ")3800 \r\n";
            FormCustomConsole.WriteLine("WaitForValueRising(" + "CH1" + ", " + 0 +
                ", " + tmpValue.ToString() + ") 3800");


            // Charger off
            com.AppendChargerOff();
            textBoxDebugInstructionPool.Text += "ChargerOff\r\n";
            FormCustomConsole.WriteLine("ChargerOff");


            // Enable fanox
            com.AppendFanoxOn();
            textBoxDebugInstructionPool.Text += "Fanox on\r\n";
            FormCustomConsole.WriteLine("Fanox on");

        }

        #endregion COMPOSITE COMMANDS
    }
}
