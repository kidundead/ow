//! @file Configure.cs


using System.Collections;


//! @class Configure
//! @brief 配置类
public class Configure
{
	//! @enum ValueType
	//! @brief 配置值类型
	private enum ValueType
	{
		Single,		//!< 单值
		Array,		//!< 一维数组
		Array2		//!< 二维数组
	}

	//! @class Value
	//! 配置项
	private class Value
	{
		public string key;
		public ValueType type;
		public string val_single;
		public ArrayList val_array = new ArrayList();
		public ArrayList val_array2 = new ArrayList();
		public string comment1;
		public string comment2;
	}

	//! @class Section
	//! 配置段
	private class Section
	{
		public string section;
		public string comment1;
		public string comment2;
		public ArrayList values = new ArrayList();
	}

	//! 配置数据
	public ArrayList m_configure = new ArrayList();


	public Configure()
	{
	}

	//! 解析配置
	public bool Load(string data)
	{
		ArrayList lines = new ArrayList();
		ArrayList last_comments = new ArrayList();
		ArrayList line_comments = new ArrayList();

		int data_len = data.Length;
		int index = 0;

		string l = "";
		string last_comment = "";

		// 解析配置
		while (true)
		{
			// get a line
			int index1, index2;
			for (index1 = index; index1 < data_len; index1++)
			{
				if ((data[index1] != '\r') && (data[index1] != '\n')) {
					break;
				}
			}
			if (index1 >= data_len)
			{
				break;
			}
			for (index2 = index1 + 1; index2 < data_len; index2++)
			{
				if ((data[index2] == '\r') || (data[index2] == '\n')) {
					break;
				}
			}

			index = index2 + 1;

			l = data.Substring(index1, index2 - index1);

			string line_comment = "";

			// 取出注释
			int comment_pos = l.IndexOf('#');
			if (comment_pos != -1)
			{
				line_comment = l.Substring(comment_pos + 1);

				l = l.Substring(0, comment_pos);
			}

			// trim " \t"
			l = l.Trim();

			// 忽略空行和无意义行
			if (l.Length < 1) {
				// 此行为一个纯注释行
				if (line_comment.Length >= 1)
				{
					last_comment = line_comment;
				}
				continue;
			}

			//
			string line = l;

			lines.Add(line);
			last_comments.Add(last_comment.Trim());
			line_comments.Add(line_comment.Trim());

			last_comment = "";
		}

		// 分析配置
		string section = "";
		for (int i = 0; i < lines.Count; i++)
		{
			string line = lines[i] as string;
	
			// 记录section
			if ((line.Length > 2) && (line[0] == '[') && (line[line.Length - 1] == ']'))
			{
				section = line.Substring(1, line.Length - 2);
				AddSection(section, last_comments[i] as string, line_comments[i] as string);
				continue;
			}

			// 取出key-value
			int pos = line.IndexOf('=');
			if (pos == -1)
			{
				continue;
			}

			string key = line.Substring(0, pos);
			string value = line.Substring(pos + 1);

			key = key.Trim();
			value = value.Trim();

			if (value == "(")
			{
				ArrayList val = new ArrayList();
				int x = i;
				for (i = i + 1; i < lines.Count; i++)
				{
					line = lines[i] as string;
					if (line[0] == ')')
					{
						break;
					}
	
					if ((line.Length > 2) && (line[0] == '(') && (line[line.Length - 1] == ')'))
					{
						string v = line.Substring(1, line.Length - 2);
						val.Add(v);
					}
					else
					{
						return false;
					}
				}

				AddValueArray2(section, key, val, last_comments[x] as string, line_comments[x] as string);
			}
			else if ((value.Length > 2) && (value[0] == '(') && (value[value.Length - 1] == ')'))
			{
				string v = value.Substring(1, value.Length - 2);
				AddValueArray(section, key, v, last_comments[i] as string, line_comments[i] as string);
			}
			else
			{
				AddValueSingle(section, key, value, last_comments[i] as string, line_comments[i] as string);
			}
		}

		return true;
	}

	//! 生成配置
	public string Save()
	{
		string data = "";
		string buf;

		for (int i = 0; i < m_configure.Count; i++)
		{
			Section sec = m_configure[i] as Section;

			if (sec.comment1.Length >= 1)
			{
				buf = string.Format("#{0}\n", sec.comment1);
				data += buf;
			}

			buf = string.Format("[{0}]", sec.section);
			data += buf;
			if (sec.comment2.Length > 1)
			{
				buf = string.Format("\t#{0}", sec.comment2);
				data += buf;
			}
			data += "\n";

			for (int j = 0; j < sec.values.Count; j++)
			{
				Value val = sec.values[j] as Value;

				if (val.comment1.Length >= 1)
				{
					buf = string.Format("#{0}\n", val.comment1);
					data += buf;
				}

				if (val.type == ValueType.Single)
				{
					buf = string.Format("{0} = {1}", val.key, val.val_single);
					data += buf;
					if (val.comment2.Length > 1)
					{
						buf = string.Format("\t#{0}", val.comment2);
						data += buf;
					}
					data += "\n";
				}
				else if (val.type == ValueType.Array)
				{
					buf = string.Format("{0} = (", val.key);
					data += buf;

					for (int m = 0; m < val.val_array.Count; m++)
					{
						if (m != (val.val_array.Count - 1)) {
							data += val.val_array[m];
							data += " ";
						}
						else {
							data += val.val_array[m];
						}
					}
					data += ")";
					if (val.comment2.Length > 1) {
						buf = string.Format("\t#{0}", val.comment2);
						data += buf;
					}
					data +="\n";
				}
				else if (val.type == ValueType.Array2)
				{
					buf = string.Format("{0} = (", val.key);
					data += buf;
					if (val.comment2.Length > 1) {
						buf = string.Format("\t#{0}", val.comment2);
						data += buf;
					}
					data += "\n";

					for (int m = 0; m < val.val_array2.Count; m++)
					{
						data += "\t(";
						
						ArrayList temp = val.val_array2[m] as ArrayList;
	
						for (int n = 0; n < temp.Count; n++)
						{
							if (n != (temp.Count - 1)) {
								data += temp[n];
								data += " ";
							}
							else {
								data += temp[n];
							}
						}

						data += ")\n";
					}

					data += ")\n";
				}
			}

			data += "\n\n";
		}

		return data;
	}

	//! 取单值配置
	//! @param section 段名称
	//! @param key 项名称
	//! @return not null:成功, null:失败
	public string GetSingle(string section, string key)
	{
		Value v = GetValue(section, key);
		if (v == null)
		{
			// key 不存在
			return null;
		}

		if (v.type != ValueType.Single)
		{
			// 值类型错误
			return null;
		}

		return v.val_single;
	}

	//! 取一维数组配置元素个数
	//! @param section 段名称
	//! @param key 项名称
	//! @return >=:成功, -1:失败
	public int CountArray(string section, string key)
	{
		Value v = GetValue(section, key);
		if (v == null)
		{
			// key 不存在
			return -1;
		}

		if (v.type != ValueType.Array)
		{
			// 值类型错误
			return -1;
		}

		return v.val_array.Count;
	}

	//! 取一维数组配置
	//! @param section 段名称
	//! @param key 项名称
	//! @param i 元素下标
	//! @return not null:成功, null:失败
	public string GetArray(string section, string key, int i)
	{
		Value v = GetValue(section, key);
		if (v == null)
		{
			// key 不存在
			return null;
		}

		if (v.type != ValueType.Array)
		{
			// 值类型错误
			return null;
		}

		int count = v.val_array.Count;
		if ((i < 0) || (i >= count))
		{
			// 下标范围错误
			return null;
		}

		return v.val_array[i] as string;
	}

	//! 取二维数组配置元素第一维个数
	//! @param section 段名称
	//! @param key 项名称
	//! @return >=:成功, -1:失败
	public int CountArray2(string section, string key)
	{
		Value v = GetValue(section, key);
		if (v == null)
		{
			// key 不存在
			return -1;
		}

		if (v.type != ValueType.Array2)
		{
			// 值类型错误
			return -1;
		}

		return v.val_array2.Count;
	}

	//! 取二维数组配置元素第二维个数
	//! @param section 段名称
	//! @param key 项名称
	//! @param i 元素第一维下标
	//! @return >=:成功, -1:失败
	public int CountArray2(string section, string key, int i)
	{
		Value v = GetValue(section, key);
		if (v == null)
		{
			// key 不存在
			return -1;
		}

		if (v.type != ValueType.Array2)
		{
			// 值类型错误
			return -1;
		}

		int c1 = v.val_array2.Count;
		if ((i < 0) || (i >= c1))
		{
			// 下标范围错误
			return -1;
		}

		ArrayList t = v.val_array2[i] as ArrayList;
		return t.Count;
	}

	//! 取二维数组配置
	//! @param section 段名称
	//! @param key 项名称
	//! @param i 元素第一维下标
	//! @param j 元素第二维下标
	//! @param val [out]配置值
	//! @return not null:成功, null:失败
	public string GetArray2(string section, string key, int i, int j)
	{
		Value v = GetValue(section, key);
		if (v == null)
		{
			// key 不存在
			return null;
		}

		if (v.type != ValueType.Array2)
		{
			// 值类型错误
			return null;
		}

		int c1 = v.val_array2.Count;
		if ((i < 0) || (i >= c1))
		{
			// 下标范围错误
			return null;
		}

		ArrayList t = v.val_array2[i] as ArrayList;

		int c2 = t.Count;
		if ((j < 0) || (j >= c2))
		{
			// 下标范围错误
			return null;
		}

		return t[j] as string;
	}

	//! 修改单值配置
	//! @param section 段名称
	//! @param key 项名称
	//! @param val 配置值
	//! @return true:成功, false:失败
	public bool SetSingle(string section, string key, string val)
	{
		Value v = GetValue(section, key);
		if (v == null)
		{
			// key 不存在
			return false;
		}

		if (v.type != ValueType.Single)
		{
			// 值类型错误
			return false;
		}

		v.val_single = val;
		return true;
	}

	//! 修改一维数组配置
	//! @param section 段名称
	//! @param key 项名称
	//! @param val 配置值
	//! @return true:成功, false:失败
	public bool SetArray(string section, string key, ArrayList val)
	{
		Value v = GetValue(section, key);
		if (v == null)
		{
			// key 不存在
			return false;
		}

		if (v.type != ValueType.Array)
		{
			// 值类型错误
			return false;
		}

		v.val_array = val;
		return true;
	}

	//! 修改二维数组配置
	//! @param section 段名称
	//! @param key 项名称
	//! @param val 配置值
	//! @return true:成功, false:失败
	public bool SetArray2(string section, string key, ArrayList val)
	{
		Value v = GetValue(section, key);
		if (v == null)
		{
			// key 不存在
			return false;
		}

		if (v.type != ValueType.Array2)
		{
			// 值类型错误
			return false;
		}

		v.val_array2 = val;
		return true;
	}

	//! 查找配置段
	//! @param section 段名称
	//! @return not null:成功, null:失败
	private Section GetSection(string section)
	{
		for (int i = 0; i < m_configure.Count; i++)
		{
			Section s = m_configure[i] as Section;
			if (s.section == section)
			{
				return s;
			}
		}

		return null;
	}

	//! 查找配置项
	//! @param section 段名称
	//! @param key 项名称
	//! @return not null:成功, null:失败
	private Value GetValue(string section, string key)
	{
		Section sec = GetSection(section);
		if (sec == null)
		{
			return null;
		}

		for (int i = 0; i < sec.values.Count; i++)
		{
			Value v = sec.values[i] as Value;
			if (v.key == key)
			{
				return v;
			}
		}

		return null;
	}

	//! 新增配置段
	//! @param section 段名称
	//! @param comment 段注释
	//! @return true:成功, false:失败
	private bool AddSection(string section, string comment1, string comment2)
	{
		Section s = GetSection(section);
		if (s != null)
		{
			// section 已经存在
			return false;
		}

		Section sec = new Section();
		sec.section = section;
		sec.comment1 = comment1;
		sec.comment2 = comment2;
		m_configure.Add(sec);
		return true;
	}

	//! 新增配置项(单值)
	//! @param section 段名称
	//! @param key 项名称
	//! @param value 配置项值
	//! @param comment 项注释
	//! @return true:成功, false:失败
	private bool AddValueSingle(string section, string key, string value, string comment1, string comment2)
	{
		Section sec = GetSection(section);
		if (sec == null)
		{
			// section 不存在
			return false;
		}

		Value v = GetValue(section, key);
		if (v != null)
		{
			// key 已存在
			return false;
		}

		Value val = new Value();
		val.key = key;
		val.type = ValueType.Single;
		val.val_single = value;
		val.comment1 = comment1;
		val.comment2 = comment2;

		sec.values.Add(val);
		return true;
	}

	//! 新增配置项(一维数组)
	//! @param section 段名称
	//! @param key 项名称
	//! @param line 数据行
	//! @param comment 项注释
	//! @return true:成功, false:失败
	private bool AddValueArray(string section, string key, string line, string comment1, string comment2)
	{
		Section sec = GetSection(section);
		if (sec == null)
		{
			// section 不存在
			return false;
		}

		Value v = GetValue(section, key);
		if (v != null)
		{
			// key 已存在
			return false;
		}

		Value val = new Value();
		val.key = key;
		val.type = ValueType.Array;
		val.comment1 = comment1;
		val.comment2 = comment2;

		// parse line
		string l = line.Replace('\t', ' ');

		while (true)
		{
			l = l.Trim();

			int pos = l.IndexOf(' ');
			if (pos != -1)
			{
				string item = l.Substring(0, pos);
				l = l.Substring(pos + 1);
				val.val_array.Add(item);
			}
			else
			{
				l = l.Trim();
				if (l.Length > 0)
				{
					val.val_array.Add(l);
				}
				break;
			}
		}

		sec.values.Add(val);
		return true;
	}

	//! 新增配置项(二维数组)
	//! @param section 段名称
	//! @param key 项名称
	//! @param lines 多个数据行
	//! @param comment 项注释
	//! @return true:成功, false:失败
	private bool AddValueArray2(string section, string key, ArrayList lines, string comment1, string comment2)
	{
		Section sec = GetSection(section);
		if (sec == null)
		{
			// section 不存在
			return false;
		}

		Value v = GetValue(section, key);
		if (v != null)
		{
			// key 已存在
			return false;
		}

		Value val = new Value();
		val.key = key;
		val.type = ValueType.Array2;
		val.comment1 = comment1;
		val.comment2 = comment2;

		// parse lines
		for (int i = 0; i < lines.Count; i++)
		{
			ArrayList val_array = new ArrayList();

			string l = lines[i] as string;
			l = l.Replace('\t', ' ');

			while (true)
			{
				l = l.Trim();

				int pos = l.IndexOf(' ');
				if (pos != -1)
				{
					string item = l.Substring(0, pos);
					l = l.Substring(pos + 1);
					val_array.Add(item);
				}
				else
				{
					l = l.Trim();
					if (l.Length > 0) {
						val_array.Add(l);
					}
					break;
				}
			}

			val.val_array2.Add(val_array);
		}

		sec.values.Add(val);
		return true;
	}
}

