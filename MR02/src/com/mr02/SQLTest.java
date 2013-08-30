package com.mr02;

import java.io.DataInput;
import java.io.DataOutput;
import java.io.IOException;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.Iterator;
import java.util.StringTokenizer;

import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.filecache.DistributedCache;
import org.apache.hadoop.fs.Path;
import org.apache.hadoop.io.LongWritable;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.io.Writable;
import org.apache.hadoop.mapred.JobConf;
import org.apache.hadoop.mapreduce.Job;
import org.apache.hadoop.mapreduce.Mapper;
import org.apache.hadoop.mapreduce.Reducer;
import org.apache.hadoop.mapreduce.lib.db.DBConfiguration;
import org.apache.hadoop.mapreduce.lib.db.DBInputFormat;
import org.apache.hadoop.mapreduce.lib.db.DBWritable;
import org.apache.hadoop.mapreduce.lib.output.FileOutputFormat;
import org.apache.hadoop.util.GenericOptionsParser;
import org.junit.Test;

/**
 * @author catch23
 * 
 */
public class SQLTest {

	/**
	 * @param args
	 * @throws Exception
	 */
	@SuppressWarnings("deprecation")
	public static void main(String[] args) throws Exception {
		// TODO Auto-generated method stub
		Configuration conf = new Configuration();
		conf.set("mapred.job.tracker", "192.168.137.131:9001");
		String[] otherArgs = new GenericOptionsParser(conf, args)
				.getRemainingArgs();
		
		
		
		Job job = new Job(conf);
		DistributedCache.addFileToClassPath(new Path( "hdfs://192.168.137.131:9000/lib/ojdbc14.jar"), job.getConfiguration()); 
		
		job.setJarByClass(SQLTest.class);
		job.setMapperClass(Map.class);
		job.setReducerClass(Reduce.class);
		
		job.setInputFormatClass(DBInputFormat.class);
		DBConfiguration.configureDB(job.getConfiguration(), "oracle.jdbc.driver.OracleDriver", "jdbc:oracle:thin:adapter/qweasd@127.0.0.1:1521:adapter");
		DBInputFormat.setInput(job, Datasets.class, "SELECT HEADER FROM ADAPTER_ORIGINALDATA", "SELECT COUNT(*) FROM ADAPTER_ORIGINALDATA");
     	job.setOutputFormatClass(FileOutputFormat.class);
		FileOutputFormat.setOutputPath(job, new Path(otherArgs[0]));
		
		job.setOutputKeyClass(Text.class);
		job.setOutputValueClass(LongWritable.class);
		
		
		System.exit(job.waitForCompletion(true) ? 0 : 1);
//		Configuration conf = new Configuration();
//		String[] otherArgs = new GenericOptionsParser(conf,args).getRemainingArgs();
//		conf.set("mapred.job.tracker", "192.168.137.131:9001");
//		
//		JobConf job = new JobConf(SQLTest.class);
//		//设置输入类型
//		job.setInputFormat(org.apache.hadoop.mapred.lib.db.DBInputFormat.class);
//		DBConfiguration.configureDB(job, "oracle.jdbc.driver.OracleDriver", "jdbc:oracle:thin:adapter/qweasd@127.0.0.1:1521:adapter");
//		
//		job.setOutputFormat(org.apache.hadoop.mapred.FileOutputFormat.class);
//		org.apache.hadoop.mapred.FileOutputFormat.setOutputPath(job,  new Path(otherArgs[0]));
//		job.setOutputKeyClass(Text.class);
//		job.setOutputValueClass(LongWritable.class);
//		
//		//job.setMapperClass((Class<? extends org.apache.hadoop.mapred.Mapper>) SQLTest.Map.class);
//		//job.setReducerClass(Reduce.class);
		
	}

	public static class Datasets implements Writable, DBWritable {
		private String eventid;
		private String header;

		@Override
		public void readFields(ResultSet result) throws SQLException {
			// TODO Auto-generated method stub
			this.header = result.getString("Header");
		}

		@Override
		public void write(PreparedStatement stmt) throws SQLException {
			// TODO Auto-generated method stub
			stmt.setString(1, this.header);
		}

		@Override
		public void readFields(DataInput in) throws IOException {
			// TODO Auto-generated method stub
			this.header = Text.readString(in);
		}

		@Override
		public void write(DataOutput out) throws IOException {
			// TODO Auto-generated method stub
			Text.writeString(out, this.header);
		}

		@Override
		public String toString() {
			// TODO Auto-generated method stub
			return this.header;
		}

	}
    
	public static class Map extends
			Mapper<LongWritable, Datasets, Text, LongWritable> {
		private static LongWritable longWritable = new LongWritable(1);
		private Text text = new Text();
		@Test
		@Override
		public void map(LongWritable key, Datasets value, Context context)
				throws IOException, InterruptedException {
			// TODO Auto-generated method stub
			super.map(key, value, context);
			StringTokenizer stringTokenizer = new StringTokenizer(
					value.toString());
			while (stringTokenizer.hasMoreTokens()) {
				text.set(stringTokenizer.nextToken());
				context.write(text, longWritable);
			}
		}

	}

	public static class Reduce extends
			Reducer<Text, LongWritable, Text, LongWritable> {
		private static LongWritable longWritable = new LongWritable();

		@Override
		public void reduce(Text key, Iterable<LongWritable> values,
				Context context) throws IOException, InterruptedException {
			// TODO Auto-generated method stub
			super.reduce(key, values, context);
			Iterator<LongWritable> iterator = values.iterator();
			long sum = 0;
			while (iterator.hasNext()) {
				sum = sum + iterator.next().get();
			}
			longWritable.set(sum);
			context.write(key, longWritable);
		}

	}
}
