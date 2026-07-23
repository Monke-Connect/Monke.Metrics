using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monke.Metrics.Models
{
	public abstract class BaseModel
	{
		/// <summary>
		/// Unique identifier for the database model.
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; init; }


		/// <summary>
		/// Creates a new <see cref="BaseModel"/> instance.
		/// </summary>
		protected BaseModel() => this.Id = 0;


		/// <summary>
		/// Creates a new <see cref="BaseModel"/> instance.
		/// </summary>
		protected BaseModel(int id)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(id);
			this.Id = id;
		}


		/// <summary>
		/// Checks wether the model hasn't already been saved/persisted.
		/// </summary>
		public bool IsTransient()
			=> this.Id == 0;


		/// <inheritdoc/>
		public override int GetHashCode() =>
			this.IsTransient() ? 0 : HashCode.Combine(this.GetType(), this.Id);


		/// <inheritdoc/>
		public override string ToString()
			=> $"{this.GetType().Name} [Id={this.Id}]";


		/// <inheritdoc/>
		public override bool Equals(object? obj)
			=> this.Equals(obj as BaseModel);


		/// <inheritdoc/>
		public bool Equals(BaseModel? other)
		{
			if (other is null)
				return false;

			if (ReferenceEquals(this, other))
				return true;

			// Transient (unsaved) entities are only equal by reference.
			if (this.IsTransient() || other.IsTransient())
				return false;

			// Compare types and id
			return this.GetType() == other.GetType() && this.Id == other.Id;
		}


		/// <inheritdoc/>
		public static bool operator ==(BaseModel? left, BaseModel? right)
			=> left is null ? right is null : left.Equals(right);


		/// <inheritdoc/>
		public static bool operator !=(BaseModel? left, BaseModel? right)
			=> !(left == right);
	}
}