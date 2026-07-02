import LoadingButton from './loading/LoadingButton';

type DeleteConfirmModalProps = {
  open: boolean;
  title: string;
  message: string;
  onConfirm: () => void;
  onCancel: () => void;
  loading?: boolean;
};

export default function DeleteConfirmModal({
  open,
  title,
  message,
  onConfirm,
  onCancel,
  loading,
}: DeleteConfirmModalProps) {
  if (!open) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 p-md loading-enter">
      <div className="bg-white rounded-xl shadow-xl max-w-md w-full p-lg content-reveal">
        <h2 className="font-headline-sm text-headline-sm font-bold text-primary mb-sm">{title}</h2>
        <p className="text-on-surface-variant mb-lg">{message}</p>
        <div className="flex justify-end gap-md">
          <button
            type="button"
            onClick={onCancel}
            disabled={loading}
            className="px-md py-sm rounded-lg border border-outline-variant hover:bg-surface-container-low disabled:opacity-50"
          >
            Cancel
          </button>
          <LoadingButton
            onClick={onConfirm}
            loading={loading}
            loadingText="Deleting…"
            className="px-md py-sm rounded-lg bg-red-600 text-white hover:bg-red-700 disabled:opacity-50"
          >
            Delete
          </LoadingButton>
        </div>
      </div>
    </div>
  );
}
