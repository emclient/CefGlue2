using System;
using System.IO;

namespace Xilium.CefGlue;

public abstract unsafe partial class CefResponseFilter
{
    private partial CefResponseFilterStatus Filter(IntPtr dataIn, nuint dataInSize, ref nuint dataInRead, IntPtr dataOut, nuint dataOutSize, ref nuint dataOutWritten)
    {
        // TODO: Use some buffers instead of UnmanagedMemoryStream.

        // TODO: Remove UnmanagedMemoryStream - normal usage is buffer operations.
        UnmanagedMemoryStream m_in_stream = null;
        UnmanagedMemoryStream m_out_stream = null;
        try
        {
            if (dataIn != IntPtr.Zero)
            {
                m_in_stream = new UnmanagedMemoryStream((byte*)dataIn, (long)dataInSize, (long)dataInSize, FileAccess.Read);
            }

            m_out_stream = new UnmanagedMemoryStream((byte*)dataOut, 0, (long)dataOutSize, FileAccess.Write);

            {
                long m_inRead;
                long m_outWritten;
                var result = Filter(m_in_stream, (long)dataInSize, out m_inRead, m_out_stream, (long)dataOutSize, out m_outWritten);
                dataInRead = (nuint)m_inRead;
                dataOutWritten = (nuint)m_outWritten;
                return result;
            }
        }
        finally
        {
            m_out_stream?.Dispose();
            m_in_stream?.Dispose();
        }
    }

    /// <summary>
    /// Called to filter a chunk of data. Expected usage is as follows:
    /// 1. Read input data from |data_in| and set |data_in_read| to the number of
    /// bytes that were read up to a maximum of |data_in_size|. |data_in| will
    /// be NULL if |data_in_size| is zero.
    /// 2. Write filtered output data to |data_out| and set |data_out_written| to
    /// the number of bytes that were written up to a maximum of
    /// |data_out_size|. If no output data was written then all data must be
    /// read from |data_in| (user must set |data_in_read| = |data_in_size|).
    /// 3. Return RESPONSE_FILTER_DONE if all output data was written or
    /// RESPONSE_FILTER_NEED_MORE_DATA if output data is still pending.
    /// This method will be called repeatedly until the input buffer has been
    /// fully read (user sets |data_in_read| = |data_in_size|) and there is no
    /// more input data to filter (the resource response is complete). This method
    /// may then be called an additional time with an empty input buffer if the
    /// user filled the output buffer (set |data_out_written| = |data_out_size|)
    /// and returned RESPONSE_FILTER_NEED_MORE_DATA to indicate that output data
    /// is still pending.
    /// Calls to this method will stop when one of the following conditions is
    /// met:
    /// 1. There is no more input data to filter (the resource response is
    /// complete) and the user sets |data_out_written| = 0 or returns
    /// RESPONSE_FILTER_DONE to indicate that all data has been written, or;
    /// 2. The user returns RESPONSE_FILTER_ERROR to indicate an error.
    /// Do not keep a reference to the buffers passed to this method.
    /// </summary>
    protected abstract CefResponseFilterStatus Filter(UnmanagedMemoryStream dataIn, long dataInSize, out long dataInRead, UnmanagedMemoryStream dataOut, long dataOutSize, out long dataOutWritten);
}
