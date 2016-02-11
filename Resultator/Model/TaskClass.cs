using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Resultator.Model
{
    [DataContract]
    public class TaskItem
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public DateTime startTime { get; set; }        
        [DataMember]
        public DateTime endTime { get; set; }
        [DataMember]
        public bool status { get; set; }
        
        public TaskItem(string tname = "", string tdesc = "")
        {          
            this.name = tname.Trim();
            this.description = tdesc.Trim();
            this.startTime = DateTime.Now;
            this.endTime = DateTime.Now.AddDays(5);
            this.status = false;
        }

    }

    [DataContract]
    public class TaskSection
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public ObservableCollection<TaskItem> items;
        
        public TaskSection(string sname = "", string sdescription = "")
        {
            this.name = sname.Trim();
            this.description = sdescription.Trim();
            this.items = new ObservableCollection<TaskItem>();
        }

        public void AddItem(TaskItem newItem)
        {
            this.items.Add(newItem);
        }       
    }

    [DataContract]
    public class TaskManager
    {
        [DataMember]
        public ObservableCollection<TaskSection> sections { get; set; }
        StorageFolder stFolder = ApplicationData.Current.LocalFolder;

        public TaskManager()
        {
            sections = new ObservableCollection<TaskSection>();
            parseFile();
        }

        //Открыть файл
        public async void parseFile()
        {
            try
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ObservableCollection<TaskSection>));
                StorageFile storageFile = await stFolder.GetFileAsync("json.txt");
                Stream str = await storageFile.OpenStreamForReadAsync();
                str.Position = 0;
                sections = (ObservableCollection<TaskSection>)ser.ReadObject(str);
            }
            catch (Exception ex)
            {
                AddSection(new TaskSection("Дом", "Домашние дела"));
                AddSection(new TaskSection("Работа", "Рабочие дела"));
                AddSection(new TaskSection("Разное", "Разные дела"));
                sections[0].AddItem(new TaskItem("Ванна", "Приклеить ленту в ванной"));
                sections[1].AddItem(new TaskItem("АСИ", "Закончить проект"));
            }

        }

        //Сохранить в файл
        public async void saveToFile()
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ObservableCollection<TaskSection>));
            MemoryStream memStream = new MemoryStream();

            ser.WriteObject(memStream, sections);
            StorageFile storageFile = await stFolder.CreateFileAsync("json.txt", CreationCollisionOption.ReplaceExisting);
            using (Stream fileStream = await storageFile.OpenStreamForWriteAsync())
            {
                memStream.Position = 0;
                await memStream.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }
        }

        //Добавить раздел
        public void AddSection(TaskSection newSection)
        {
            this.sections.Add(newSection);
        }

        //Получить все задачи одним списком
        public TaskSection GetAllItems()
        {
            TaskSection allSection = new TaskSection();
            foreach (TaskSection section in this.sections)
            {
                foreach (TaskItem item in section.items)
                {
                    if (!item.status) allSection.items.Add(item);
                }
            }
            return allSection;
        }

    }

    
}
