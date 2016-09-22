//! @file DampedVibration.cs


using UnityEngine;


//! @class DampedVibration
//! @brief ×èÄáÕñ¶¯
public class DampedVibration
{
	// S = A * e^(-beta * t) * cos(omega * t + alpha)

	private const float e = 2.718281828f;

	private float m_A;
	private float m_beta;
	private float m_omega;
	private float m_alpha;

	public void SetParameter(float A, float beta, float omega, float alpha)
	{
		m_A = A;
		m_beta = beta;
		m_omega = omega;
		m_alpha = alpha;
	}

	public float CalculateDistance(float time)
	{
		return m_A * Mathf.Pow(e, -m_beta * time) * Mathf.Cos(m_omega * time + m_alpha);
	}

	public float CalculateZeroTime(int n)
	{
		float delta = m_alpha / Mathf.PI - 0.5f;

		int rn;
		if (m_omega > 0)
		{
			rn = Mathf.CeilToInt(delta) + (n - 1);
		}
		else
		{
			rn = Mathf.FloorToInt(delta) - (n - 1);
		}

		float time = ((Mathf.PI / 2.0f) * (1 + 2 * rn) - m_alpha) / m_omega;
		return time;
	}
}

